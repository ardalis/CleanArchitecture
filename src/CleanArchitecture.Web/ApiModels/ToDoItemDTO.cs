using CleanArchitecture.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Web.ApiModels
{
    // Note: doesn't expose events or behavior
    public class ToDoItemDTO
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; private set; }

        public static ToDoItemDTO FromToDoItem(ToDoItem item)
        {
            return new ToDoItemDTO()
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                IsDone = item.IsDone
            };
        }
    }
}
