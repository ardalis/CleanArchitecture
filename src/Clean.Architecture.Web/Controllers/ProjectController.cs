using System.Linq;
using System.Threading.Tasks;
using Clean.Architecture.Core;
using Clean.Architecture.Core.ProjectAggregate;
using Clean.Architecture.Core.ProjectAggregate.Specifications;
using Clean.Architecture.SharedKernel.Interfaces;
using Clean.Architecture.Web.ApiModels;
using Microsoft.AspNetCore.Mvc;

namespace Clean.Architecture.Web.Controllers
{
    [Route("{controller}")]
    public class ProjectController : Controller
    {
        private readonly IRepository<Project> _projectRepository;

        public ProjectController(IRepository<Project> projectRepository)
        {
            _projectRepository = projectRepository;
        }

        // GET project/{projectId?}
        [HttpGet("{projectId:int}")]
        public async Task<IActionResult> Index(int projectId = 1)
        {
            var spec = new ProjectByIdWithItemsSpec(projectId);
            var project = await _projectRepository.GetBySpecAsync(spec);

            var dto = new ProjectDTO
            {
                Id = project.Id,
                Name = project.Name,
                Items = project.Items
                            .Select(item => ToDoItemDTO.FromToDoItem(item))
                            .ToList()
            };
            return View(dto);
        }

        // GET project/populate
        [HttpGet("populate")]
        public IActionResult Populate()
        {
            int recordsAdded = DatabasePopulator.PopulateDatabase(_projectRepository);
            return Ok(recordsAdded);
        }
    }
}
