using System.Linq;
using Clean.Architecture.Core.ProjectAggregate;
using Clean.Architecture.SharedKernel.Interfaces;

namespace Clean.Architecture.Core
{
    public static class DatabasePopulator
    {
        public static int PopulateDatabase(IRepository<Project> projectRepository)
        {
            if (projectRepository.ListAsync().Result.Count() > 0) return 0;

            var project = new Project("Get familiar with Clean Architecture template");

            project.AddItem(new ToDoItem
            {
                Title = "Get Sample Working",
                Description = "Try to get the sample to build."
            });
            project.AddItem(new ToDoItem
            {
                Title = "Review Solution",
                Description = "Review the different projects in the solution and how they relate to one another."
            });
            project.AddItem(new ToDoItem
            {
                Title = "Run and Review Tests",
                Description = "Make sure all the tests run and review what they are doing."
            });

            projectRepository.AddAsync(project).Wait();

            return projectRepository.ListAsync().Result.Count;
        }
    }
}
