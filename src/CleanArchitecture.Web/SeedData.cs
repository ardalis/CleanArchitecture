using CleanArchitecture.Core.Entities;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace CleanArchitecture.Web
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var dbContext = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                // Look for any TODO items.
                if (dbContext.ToDoItems.Any())
                {
                    return;   // DB has been seeded
                }

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
                dbContext.SaveChanges();

            }
        }
        public static void PopulateTestData(AppDbContext dbContext)
        {
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
            dbContext.SaveChanges();
        }

    }
}
