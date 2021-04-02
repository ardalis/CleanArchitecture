using System.Collections.Generic;

namespace Clean.Architecture.Web.ApiModels
{
    public class ProjectDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ToDoItemDTO> Items = new();
    }
}
