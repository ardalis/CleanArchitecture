using LiteDB;
using LiteDB.Async;

namespace Clean.Architecture.Infrastructure.LiteDBData;

public class LiteDbContext
{
  private const string DatabaseLocation = "./LiteDb.db";
  public LiteDatabaseAsync Database { get; }

  public LiteDbContext()
  {
    Database = new LiteDatabaseAsync(DatabaseLocation);
  }
}
