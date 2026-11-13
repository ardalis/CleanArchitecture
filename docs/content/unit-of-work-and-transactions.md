---
title: "Unit of Work & Transactions"
weight: 55
---

# Unit of Work & Transactions

Sometimes a single use case has to change several aggregates, and those changes must succeed or fail together. The usual tools for this are the **Unit of Work** pattern and database **transactions**. This page explains what the template does by default and how to add your own unit of work when you need one.

## The Default Behavior

The sample repository, `EfRepository<T>`, inherits from `RepositoryBase<T>` in [Ardalis.Specification.EntityFrameworkCore](https://github.com/ardalis/specification). **It does not support a unit of work or transactions that span calls.** Every write method saves right away:

| Method | What it does |
|---|---|
| `AddAsync` / `AddRangeAsync` | Adds the entity, then calls `SaveChangesAsync` |
| `UpdateAsync` / `UpdateRangeAsync` | Marks the entity modified, then calls `SaveChangesAsync` |
| `DeleteAsync` / `DeleteRangeAsync` | Removes the entity, then calls `SaveChangesAsync` |

That is simple and works well when a command touches one aggregate, which is the case for every handler in the sample. A handler that makes several repository calls, though, makes several separate trips to the database:

```csharp
// Each call is persisted on its own. If the second call fails,
// the first change is already committed.
await _orderRepository.UpdateAsync(order, ct);
await _inventoryRepository.UpdateAsync(inventoryItem, ct);
```

Each `SaveChangesAsync` call is atomic by itself, because EF Core wraps it in a transaction. But there is no transaction around both calls together.

## Why a Unit of Work Is Easy to Add

EF Core's `DbContext` already *is* a unit of work. It tracks every entity you add, change, or remove, and writes all of them in a single transaction when you call `SaveChangesAsync`.

`AddDbContext` registers `AppDbContext` as **scoped**, and the repositories are scoped too. So during one HTTP request, every repository a handler uses shares the **same** `AppDbContext` instance. All you need is control over *when* `SaveChangesAsync` is called. Instead of saving inside each repository method, save once, at the end of the use case.

## Building Your Own Unit of Work

The steps below are one simple way to do it. Change the names and the location of the files to fit your solution.

### 1. Define the abstraction

Put the interface where your use cases can depend on it, such as the UseCases project or `Core/Interfaces`. It should not mention EF Core.

```csharp
namespace Clean.Architecture.UseCases.Interfaces;

public interface IUnitOfWork
{
  Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
```

### 2. Implement it in Infrastructure

The implementation just hands the call to the scoped `AppDbContext`:

```csharp
namespace Clean.Architecture.Infrastructure.Data;

public class EfUnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
  public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
    dbContext.SaveChangesAsync(cancellationToken);
}
```

### 3. Make the repository stage changes instead of saving them

All write methods on `RepositoryBase<T>` are `virtual`, so you can override them to change tracked state without calling `SaveChangesAsync`:

```csharp
namespace Clean.Architecture.Infrastructure.Data;

public class EfRepository<T>(AppDbContext dbContext) :
  RepositoryBase<T>(dbContext), IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
{
  public override Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
  {
    DbContext.Set<T>().Add(entity);
    return Task.FromResult(entity);
  }

  public override Task<int> UpdateAsync(T entity, CancellationToken cancellationToken = default)
  {
    DbContext.Set<T>().Update(entity);
    return Task.FromResult(0);
  }

  public override Task<int> DeleteAsync(T entity, CancellationToken cancellationToken = default)
  {
    DbContext.Set<T>().Remove(entity);
    return Task.FromResult(0);
  }

  // Override AddRangeAsync, UpdateRangeAsync and DeleteRangeAsync the same way.
}
```

If you'd rather keep the default behavior for most of the app, put these overrides in a separate repository type, for example `EfStagingRepository<T>`, and use it only where you need it.

### 4. Register it

```csharp
services.AddScoped<IUnitOfWork, EfUnitOfWork>();
```

Use the default scoped lifetime. A singleton or transient `DbContext` would break the "one context per request" assumption this approach relies on.

### 5. Save once, at the end of the use case

```csharp
public class PlaceOrderHandler(
  IRepository<Order> _orderRepository,
  IRepository<InventoryItem> _inventoryRepository,
  IUnitOfWork _unitOfWork)
  : ICommandHandler<PlaceOrderCommand, Result<OrderId>>
{
  public async ValueTask<Result<OrderId>> Handle(PlaceOrderCommand command, CancellationToken ct)
  {
    var item = await _inventoryRepository.GetByIdAsync(command.ItemId, ct);
    if (item is null) return Result.NotFound();

    item.Reserve(command.Quantity);
    var order = new Order(command.CustomerId, item.Id, command.Quantity);

    await _orderRepository.AddAsync(order, ct);
    await _inventoryRepository.UpdateAsync(item, ct);

    // One call, one transaction: both changes commit, or neither does.
    await _unitOfWork.SaveChangesAsync(ct);

    return order.Id;
  }
}
```

> **Note:** If your repository no longer saves, *every* command has to call `IUnitOfWork.SaveChangesAsync`, or nothing is written. To save in one place instead, call it from a Mediator pipeline behavior that runs after each command handler succeeds. Then individual handlers don't need to remember it.

## Explicit Transactions

Sometimes one `SaveChangesAsync` is not enough. For example, you might need a database-generated value before you can write the next record, or you might mix EF Core with raw SQL or `ExecuteUpdateAsync`. In those cases, you can add explicit transaction methods to your abstraction and implement them with `DbContext.Database`:

```csharp
public interface IUnitOfWork
{
  Task BeginTransactionAsync(CancellationToken cancellationToken = default);
  Task CommitTransactionAsync(CancellationToken cancellationToken = default);
  Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
  Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
```

```csharp
public class EfUnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
  private IDbContextTransaction? _transaction;

  public async Task BeginTransactionAsync(CancellationToken ct = default) =>
    _transaction = await dbContext.Database.BeginTransactionAsync(ct);

  public async Task CommitTransactionAsync(CancellationToken ct = default)
  {
    if (_transaction is null) return;
    await _transaction.CommitAsync(ct);
    await _transaction.DisposeAsync();
    _transaction = null;
  }

  public async Task RollbackTransactionAsync(CancellationToken ct = default)
  {
    if (_transaction is null) return;
    await _transaction.RollbackAsync(ct);
    await _transaction.DisposeAsync();
    _transaction = null;
  }

  public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
    dbContext.SaveChangesAsync(ct);
}
```

A few things to keep in mind:

- **Prefer a single `SaveChangesAsync` when you can.** It is already transactional and needs none of this extra code.
- **Retrying execution strategies.** If you turn on connection resiliency, such as `EnableRetryOnFailure` for SQL Server, EF Core won't let you open your own transaction directly. Run the whole transaction inside `dbContext.Database.CreateExecutionStrategy().ExecuteAsync(...)` so it can be retried as a unit.
- **Domain events.** The template's `EventDispatchInterceptor` sends domain events *after each successful `SaveChangesAsync`*. With one `SaveChangesAsync` per use case, events go out once, after the data is committed. Inside an explicit transaction, though, events are sent after each save, *before* the transaction commits. If you roll back later, handlers may already have acted on changes that never happened. Design your handlers for that, or save only once per transaction.

## Summary

- The default `EfRepository<T>` saves on every write call, so there is no unit of work or transaction spanning several repository calls.
- `AppDbContext` is scoped and shared by all repositories in a request, and it already works as a unit of work.
- To make several changes atomic, stop saving inside the repository, add a small `IUnitOfWork` abstraction, and call `SaveChangesAsync` once at the end of the use case (or in a pipeline behavior).
- Use explicit transactions only when one `SaveChangesAsync` can't cover the work.

The same approach works for the [Minimal Clean Architecture](templates/minimal-clean.md) template, which uses the same `RepositoryBase<T>`-based repository.
