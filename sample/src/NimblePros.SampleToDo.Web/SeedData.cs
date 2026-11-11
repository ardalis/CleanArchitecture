using NimblePros.SampleToDo.Core.ContributorAggregate;
using NimblePros.SampleToDo.Core.ProjectAggregate;
using NimblePros.SampleToDo.Infrastructure.Data;

namespace NimblePros.SampleToDo.Web;

public static class SeedData
{
  public static readonly Contributor Contributor1 = new(ContributorName.From("Ardalis"));
  public static readonly Contributor Contributor2 = new(ContributorName.From("Snowfrog"));
  public static readonly Project TestProject1 = new Project(ProjectName.From("Test Project"));

  public static readonly ToDoItem ToDoItem1 =
    new ToDoItem(title: ToDoItemTitle.From("Get Sample Working"),
      description: ToDoItemDescription.From("Try to get the sample to build."));

  public static readonly ToDoItem ToDoItem2 =
  new ToDoItem(title: ToDoItemTitle.From("Review Solution"),
    description: ToDoItemDescription.From("Review the different projects in the solution and how they relate to one another."));


  public static readonly ToDoItem ToDoItem3 =
new ToDoItem(title: ToDoItemTitle.From("Run and Review Tests"),
  description: ToDoItemDescription.From("Make sure all the tests run and review what they are doing."));

  /// <summary>
  /// Seeds sample data if the ToDoItems table is empty.
  /// WARNING: This is for local development and demos only. If ToDoItems is empty,
  /// it calls <see cref="PopulateTestDataAsync"/>, which DELETES all Projects, ToDoItems
  /// and Contributors. Do not run this against a shared or production database
  /// (e.g. a dev build pointed at a production connection string). Before using this
  /// sample as the basis for a real application, remove this seeding or restrict it
  /// to the Development environment and base it on your own data.
  /// See https://github.com/ardalis/CleanArchitecture/issues/953
  /// </summary>
  public static async Task InitializeAsync(AppDbContext dbContext)
  {
    if (await dbContext.ToDoItems.AnyAsync())
    {
      return;   // DB has been seeded
    }

    await PopulateTestDataAsync(dbContext);
  }

  /// <summary>
  /// DESTRUCTIVE: deletes all existing Projects, ToDoItems and Contributors, then inserts
  /// sample data. Intended for local development and tests only - never for production data.
  /// </summary>
  public static async Task PopulateTestDataAsync(AppDbContext dbContext)
  {
    foreach (var item in dbContext.Projects)
    {
      dbContext.Remove(item);
    }
    foreach (var item in dbContext.ToDoItems)
    {
      dbContext.Remove(item);
    }
    foreach (var item in dbContext.Contributors)
    {
      dbContext.Remove(item);
    }
    await dbContext.SaveChangesAsync();

    dbContext.Contributors.Add(Contributor1);
    dbContext.Contributors.Add(Contributor2);

    await dbContext.SaveChangesAsync();

    ToDoItem1.AddContributor(Contributor1.Id);
    ToDoItem2.AddContributor(Contributor2.Id);
    ToDoItem3.AddContributor(Contributor1.Id);

    TestProject1.AddItem(ToDoItem1);
    TestProject1.AddItem(ToDoItem2);
    TestProject1.AddItem(ToDoItem3);
    dbContext.Projects.Add(TestProject1);

    await dbContext.SaveChangesAsync();

    // add a bunch more contributors to support demonstrating paging
    for (int i = 1; i <= 25; i++)
    {
      dbContext.Contributors.Add(new Contributor(ContributorName.From($"Contributor {i}")));
    }
    await dbContext.SaveChangesAsync();
  }
}
