using System.Linq;
using Clean.Architecture.Core.Entities;
using Clean.Architecture.SharedKernel.Interfaces;

namespace Clean.Architecture.Core
{
    public static class DatabasePopulator
    {
        public static int PopulateDatabase(IRepository todoRepository)
        {
            if (todoRepository.ListAsync<ToDoItem>().Result.Count() >= 3) return 0;

            todoRepository.AddAsync(new ToDoItem
            {
                Title = "Get Sample Working",
                Description = "Try to get the sample to build."
            }).Wait();
            todoRepository.AddAsync(new ToDoItem
            {
                Title = "Review Solution",
                Description = "Review the different projects in the solution and how they relate to one another."
            }).Wait();
            todoRepository.AddAsync(new ToDoItem
            {
                Title = "Run and Review Tests",
                Description = "Make sure all the tests run and review what they are doing."
            }).Wait();

            return todoRepository.ListAsync<ToDoItem>().Result.Count;
        }
    }
}
