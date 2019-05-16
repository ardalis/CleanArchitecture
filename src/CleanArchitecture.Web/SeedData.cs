using CleanArchitecture.Core.Entities;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CleanArchitecture.Web
{
    public static class SeedData
    {
        public static void PopulateTestData(AppDbContext dbContext)
        {
            if (dbContext.ToDoItems.Any())
            {
                Foo single = dbContext.Foos.Include("Bar").Single();
                single.Name = "Steve";
                single.Bar.Number = 5;
                dbContext.SaveChanges();
                return;
            }
            var toDos = dbContext.ToDoItems;
            foreach (var item in toDos)
            {
                dbContext.Remove(item);
            }
            dbContext.SaveChanges();
            dbContext.ToDoItems.Add(new ToDoItem()
            {
                Title = "Test Item 1",
                Description = "Test Description One"
            });
            dbContext.ToDoItems.Add(new ToDoItem()
            {
                Title = "Test Item 2",
                Description = "Test Description Two"
            });

            dbContext.Foos.Add(new Foo { Name = "Steve", Bar = new Bar { Number = 5 } });
            dbContext.SaveChanges();
        }

    }
}
