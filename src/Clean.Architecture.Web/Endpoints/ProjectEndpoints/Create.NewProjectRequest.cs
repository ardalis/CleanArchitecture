using System.ComponentModel.DataAnnotations;

namespace Clean.Architecture.Web.Endpoints.ProjectEndpoints
{
    public class NewProjectRequest
    {
        [Required]
        public string Name { get; set; }
    }
}