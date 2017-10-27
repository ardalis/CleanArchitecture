using CleanArchitecture.Core.Entities;

namespace CleanArchitecture.Infrastructure.Data
{
    public class SeedData
    {
        private readonly AppDbContext _dbContext;

        public SeedData(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void PopulateTestData()
        {
            var toDos = _dbContext.ToDoItems;
            foreach (var item in toDos)
            {
                _dbContext.Remove(item);
            }
            _dbContext.SaveChanges();
            _dbContext.ToDoItems.Add(new ToDoItem()
            {
                Title = "Test Item 1",
                Description = "Test Description One"
            });
            _dbContext.ToDoItems.Add(new ToDoItem()
            {
                Title = "Test Item 2",
                Description = "Test Description Two"
            });
            _dbContext.SaveChanges();
        }

    }
}
