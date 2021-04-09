using System.ComponentModel.DataAnnotations;

namespace Clean.Architecture.Web.Endpoints.ProjectEndpoints
{
    public class UpdateProjectRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}