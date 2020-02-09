using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Web.Endpoints.ToDoItems
{
    public class CreateCommand
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
