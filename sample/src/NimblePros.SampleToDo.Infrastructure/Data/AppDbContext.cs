using NimblePros.SampleToDo.Core.ContributorAggregate;
using NimblePros.SampleToDo.Core.ProjectAggregate;

namespace NimblePros.SampleToDo.Infrastructure.Data;

public class AppDbContext : DbContext
{

  public AppDbContext(DbContextOptions<AppDbContext> options)
      : base(options) {}

  public DbSet<ToDoItem> ToDoItems => Set<ToDoItem>();
  public DbSet<Project> Projects => Set<Project>();
  public DbSet<Contributor> Contributors => Set<Contributor>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }

  protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
  {
    base.ConfigureConventions(configurationBuilder);

    //configurationBuilder.RegisterAllInVogenEfCoreConverters();
  }

  public override int SaveChanges()
  {
    return SaveChangesAsync().GetAwaiter().GetResult();
  }
}
