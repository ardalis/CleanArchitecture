using Microsoft.Data.Sqlite;
using NimblePros.SampleToDo.Core.ContributorAggregate;
using NimblePros.SampleToDo.Infrastructure.Data;

namespace NimblePros.SampleToDo.IntegrationTests.Data;

/// <summary>
/// Runs against a real relational provider (in-memory SQLite) rather than the EF Core
/// InMemory provider. Relational providers assign temporary negative keys to new
/// entities until SaveChanges, which must not trip ContributorId validation (#1195).
/// </summary>
public sealed class SqliteContributorPersistence : IDisposable
{
  private readonly SqliteConnection _connection = new("DataSource=:memory:");
  private readonly DbContextOptions<AppDbContext> _options;

  public SqliteContributorPersistence()
  {
    _connection.Open();
    _options = new DbContextOptionsBuilder<AppDbContext>()
      .UseSqlite(_connection)
      .Options;

    using var db = new AppDbContext(_options);
    db.Database.EnsureCreated();
  }

  [Fact]
  public async Task AddsContributorWithDatabaseGeneratedId()
  {
    var cancellationToken = TestContext.Current.CancellationToken;

    await using (var db = new AppDbContext(_options))
    {
      var repository = new EfRepository<Contributor>(db);
      await repository.AddAsync(new Contributor(ContributorName.From("New Contributor")), cancellationToken);
    }

    await using var readDb = new AppDbContext(_options);
    var saved = await readDb.Contributors.SingleAsync(cancellationToken);

    Assert.True(saved.Id.Value > 0);
    Assert.Equal("New Contributor", saved.Name.Value);
  }

  [Fact]
  public void ContributorIdFromStillRejectsNonPositiveValues()
  {
    Assert.Throws<Vogen.ValueObjectValidationException>(() => ContributorId.From(0));
    Assert.Throws<Vogen.ValueObjectValidationException>(() => ContributorId.From(-1));
  }

  public void Dispose() => _connection.Dispose();
}
