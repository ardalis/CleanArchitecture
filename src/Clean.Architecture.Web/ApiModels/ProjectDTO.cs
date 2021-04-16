using System.Collections.Generic;

namespace Clean.Architecture.Web.ApiModels
{
    public class ProjectDTO : CreateProjectDTO
    {
        public int Id { get; set; }
        public List<ToDoItemDTO> Items = new();
    }

    public class CreateProjectDTO
    {
        public string Name { get; set; }
    }
}
