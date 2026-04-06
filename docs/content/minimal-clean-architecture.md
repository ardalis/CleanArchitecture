# Minimal Clean Architecture

## Overview

The **Minimal Clean Architecture** template provides a simplified, pragmatic approach to Clean Architecture for ASP.NET Core applications. It maintains the core principles of Clean Architecture—separation of concerns, dependency inversion, and testability—while reducing complexity through a single-project Vertical Slice Architecture (VSA).

## Philosophy

### Core Principles

1. **Simplicity First**: Minimize unnecessary abstractions and project boundaries
2. **Vertical Slices**: Organize by feature rather than technical layer
3. **Pragmatic DDD**: Use domain patterns where they add value, not everywhere
4. **Progressive Enhancement**: Start simple, add complexity only when needed

### Clean Architecture Principles Maintained

- ✅ **Dependency Inversion**: Domain doesn't depend on infrastructure
- ✅ **Testability**: Business logic can be tested in isolation
- ✅ **Separation of Concerns**: Clear boundaries between domain, infrastructure, and presentation
- ✅ **Framework Independence**: Domain logic isn't coupled to ASP.NET Core

### Simplifications from Full Template

- **Single Project**: All code in one Web project instead of 4+ projects
- **Simplified DDD**: Essential patterns only (entities, aggregates) without extensive value objects, specifications
- **Optional CQRS**: Mediator is optional; logic can live in endpoints
- **Direct Data Access**: Can use DbContext directly or simple repositories instead of complex repository pattern
- **Vertical Organization**: Group by feature (Cart, Order, Product) instead of by layer

## Architecture

### Project Structure

```text
MinimalClean.Architecture.Web/
├── Domain/                         # Domain Layer
│   ├── CartAggregate/
│   │   ├── Cart.cs                 # Aggregate root
│   │   ├── CartItem.cs            # Entity
│   │   └── Events/                # Domain events (optional)
│   ├── OrderAggregate/
│   └── ProductAggregate/
├── Infrastructure/                 # Infrastructure Layer
│   ├── Data/
│   │   ├── AppDbContext.cs        # EF Core DbContext
│   │   ├── Config/                # EF configurations
│   │   │   ├── CartConfiguration.cs
│   │   │   └── OrderConfiguration.cs
│   │   └── Migrations/            # EF migrations
│   ├── Email/                     # External services
│   └── Services/                  # Infrastructure services
├── Endpoints/                      # Presentation Layer
│   ├── Cart/
│   │   ├── Create.cs              # Create cart endpoint
│   │   ├── AddItem.cs             # Add item to cart
│   │   └── List.cs                # List carts
│   ├── Order/
│   └── Product/
└── Program.cs                     # Application startup
```

### Vertical Slice Organization

Each feature (Cart, Order, Product) contains:

- **Domain**: Entities and business logic
- **Infrastructure**: Data configurations for that feature
- **Endpoints**: API endpoints for that feature

This keeps related code together, making it easier to understand and modify features independently.

### Dependency Flow

```text
Endpoints ──→ Domain
    ↓
Infrastructure ──→ Domain
```

- Endpoints can use Domain and Infrastructure
- Infrastructure depends on Domain (for entity configurations)
- Domain has no dependencies on other layers

## Design Decisions

### ADR-001: Single Project Architecture

**Status**: Accepted

**Context**: Need to balance architectural guidance with simplicity for smaller applications.

**Decision**: Use a single Web project with clear folder structure instead of multiple projects.

**Consequences**:
- ✅ Simpler to navigate and understand
- ✅ Faster builds (no project-to-project references)
- ✅ Easier to refactor (no project boundary concerns)
- ✅ Lower initial complexity
- ⚠️ Developers must respect folder boundaries (not enforced by compiler)
- ⚠️ Harder to enforce strict layer separation

**Migration Path**: Can be extracted into multiple projects later if needed.

### ADR-002: Vertical Slice Organization

**Status**: Accepted

**Context**: Need to organize code in a way that's easy to understand and modify.

**Decision**: Organize by feature (vertical slices) rather than by layer (horizontal).

**Consequences**:
- ✅ Related code is colocated (easier to find)
- ✅ Features can be modified independently
- ✅ Natural fit for microservices extraction
- ✅ Aligns with business capabilities
- ⚠️ May have some code duplication across features
- ⚠️ Shared concerns need careful consideration

### ADR-003: Pragmatic DDD

**Status**: Accepted

**Context**: Full DDD patterns can be overkill for simpler domains.

**Decision**: Use essential DDD patterns (entities, aggregates) but keep it simple.

**Patterns Included**:
- ✅ Entities with encapsulation
- ✅ Aggregate roots
- ✅ Domain events (optional)

**Patterns Simplified/Optional**:
- ⚠️ Value Objects (use when valuable, not everywhere)
- ⚠️ Specifications (use LINQ, add if needed)
- ⚠️ Domain Services (add only when needed)

**Consequences**:
- ✅ Easier to learn and apply
- ✅ Less boilerplate code
- ✅ Faster development
- ⚠️ May need to add patterns as domain complexity grows

### ADR-004: Optional Mediator/CQRS

**Status**: Accepted

**Context**: CQRS with Mediator adds valuable patterns but also complexity.

**Decision**: Make Mediator optional; allow business logic in endpoints for simple cases. Trade-off is no ability to use custom pipeline for cross-cutting concerns.

**Usage Guidelines**:
- Simple CRUD: Can put logic directly in endpoints
- Complex workflows: Use Mediator commands/queries
- Cross-cutting concerns: Use Mediator pipeline behaviors

**Consequences**:
- ✅ Lower initial complexity
- ✅ Developers choose appropriate level of abstraction
- ⚠️ Inconsistent patterns across codebase possible
- ⚠️ Need clear team guidelines on when to use Mediator

### ADR-005: FastEndpoints for APIs

**Status**: Accepted

**Context**: Need clean, testable API endpoints.

**Decision**: Use FastEndpoints with REPR pattern.

**Consequences**:
- ✅ One file per endpoint (easy to find)
- ✅ Built-in validation support
- ✅ Clear request/response types
- ✅ Testable without HTTP layer
- ⚠️ Different from standard ASP.NET Core patterns
- ⚠️ Learning curve for team

## When to Use This Template

### Ideal Scenarios

1. **MVPs and Prototypes**
   - Need to validate ideas quickly
   - Want architectural guidance without overhead
   - May grow into larger application

2. **Small to Medium Applications**
   - 5-50 endpoints
   - 5-20 domain entities
   - 1-5 developers
   - Simple to moderate domain complexity

3. **Learning Clean Architecture**
   - Want to understand principles without complexity
   - Stepping stone to full Clean Architecture
   - Teaching tool for teams

4. **Vertical Slice Architecture Preference**
   - Team prefers feature-based organization
   - Planning to extract microservices later
   - Value cohesion over layer separation

### Not Recommended For

1. **Large Enterprise Applications**
   - Complex domain requiring extensive DDD patterns
   - Multiple teams needing strict boundaries
   - Long-term evolution expected
   - → Use Full Clean Architecture instead

2. **Microservices from Start**
   - If you know you'll split into services
   - → Consider separate services with minimal template each

3. **Regulatory/Compliance Heavy**
   - Strict audit requirements
   - Need enforced layer boundaries
   - → Use Full Clean Architecture instead

## Migration Paths

### From Minimal to Full Clean Architecture

As your application grows, you can migrate to the full template:

#### Step 1: Extract Core Project

```powershell
# Create new Core project
dotnet new classlib -n YourProject.Core

# Move domain entities
mv Domain/* ../YourProject.Core/

# Update namespaces
# Update project references
```

#### Step 2: Extract Infrastructure Project

```powershell
# Create Infrastructure project
dotnet new classlib -n YourProject.Infrastructure

# Move infrastructure code
mv Infrastructure/* ../YourProject.Infrastructure/

# Add reference to Core
dotnet add YourProject.Infrastructure reference YourProject.Core
```

#### Step 3: Extract UseCases (Optional)

```powershell
# Create UseCases project
dotnet new classlib -n YourProject.UseCases

# Move business logic from endpoints to use cases
# Add Mediator (if not already using)
# Create command/query handlers
# Leverage Mediator Behaviors for cross-cutting concerns
```

#### Step 4: Clean Up Web Project

- Update project references
- Keep only endpoints and startup code
- Reference UseCases or Infrastructure as needed

### From Full Clean Architecture to Minimal

If you find the full template too complex:

#### Step 1: Merge Projects

```powershell
# Copy all code into Web project
# Organize by vertical slices
```

#### Step 2: Simplify Patterns

- Replace Specifications with LINQ
- Simplify Value Objects to primitives where beneficial
- Remove unnecessary abstractions

#### Step 3: Organize Vertically

- Group by feature instead of layer
- Colocate related code

## Best Practices

### Domain Layer

```csharp
// Good: Encapsulated entity
public class Cart
{
    private readonly List<CartItem> _items = new();
    public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();
    
    public void AddItem(Product product, int quantity)
    {
        // Business logic here
        var existingItem = _items.FirstOrDefault(i => i.ProductId == product.Id);
        if (existingItem != null)
        {
            existingItem.IncreaseQuantity(quantity);
        }
        else
        {
            _items.Add(new CartItem(product, quantity));
        }
    }
}

// Avoid: Anemic domain model
public class Cart
{
    public List<CartItem> Items { get; set; } = new();
}
```

### Endpoint Layer

```csharp
// Good: Clear, focused endpoint
public class CreateCart : EndpointWithoutRequest<CartResponse>
{
    private readonly AppDbContext _db;
    
    public CreateCart(AppDbContext db) => _db = db;
    
    public override void Configure()
    {
        Post("/carts");
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(CancellationToken ct)
    {
        var cart = new Cart(guestUserId: Guid.NewGuid());
        _db.Carts.Add(cart);
        await _db.SaveChangesAsync(ct);
        
        await Send.Async(new CartResponse(cart.Id, cart.Items.Count), cancellation: ct);
    }
}
```

### Infrastructure Layer

```csharp
// Good: Focused EF configuration
public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.HasKey(c => c.Id);
        builder.HasMany(c => c.Items)
               .WithOne()
               .HasForeignKey("CartId");
    }
}
```

## Testing Strategy

### Unit Tests

Focus on domain logic:

```csharp
public class CartTests
{
    [Fact]
    public void AddItem_NewProduct_AddsToCart()
    {
        // Arrange
        var cart = new Cart(guestUserId: Guid.NewGuid());
        var product = new Product("Test", 10m);
        
        // Act
        cart.AddItem(product, 2);
        
        // Assert
        Assert.Single(cart.Items);
        Assert.Equal(2, cart.Items.First().Quantity);
    }
}
```

### Functional Tests

Test endpoints end-to-end:

```csharp
public class CartEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task CreateCart_ReturnsNewCart()
    {
        // Arrange
        var client = _factory.CreateClient();
        
        // Act
        var response = await client.PostAsync("/carts", null);
        
        // Assert
        response.EnsureSuccessStatusCode();
        var cart = await response.Content.ReadFromJsonAsync<CartResponse>();
        Assert.NotNull(cart);
    }
}
```

## Resources

- [Main Clean Architecture Template](https://github.com/ardalis/CleanArchitecture)
- [Vertical Slice Architecture](https://jimmybogard.com/vertical-slice-architecture/)
- [FastEndpoints Documentation](https://fast-endpoints.com/)
- [Domain-Driven Design Fundamentals](https://www.pluralsight.com/courses/fundamentals-domain-driven-design)
- [Clean Architecture Course](https://academy.nimblepros.com/p/intro-to-clean-architecture)

## Contributing

Contributions are welcome! Please see the main [Contributing Guide](../CONTRIBUTING.md).

## License

MIT - see [LICENSE](../LICENSE)
