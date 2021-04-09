using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Clean.Architecture.Core.ProjectAggregate;
using Clean.Architecture.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Clean.Architecture.Web.Endpoints.ProjectEndpoints
{

    public class List : BaseAsyncEndpoint
        .WithoutRequest
        .WithResponse<ProjectListResponse>
    {
        private readonly IRepository<Project> _repository;

        public List(IRepository<Project> repository)
        {
            _repository = repository;
        }

        [HttpGet("/ToDoItems")]
        [SwaggerOperation(
            Summary = "Gets a list of all ToDoItems",
            Description = "Gets a list of all ToDoItems",
            OperationId = "ToDoItem.List",
            Tags = new[] { "ToDoItemEndpoints" })
        ]
        public override async Task<ActionResult<ProjectListResponse>> HandleAsync(CancellationToken cancellationToken)
        {
            var response = new ProjectListResponse();
            response.Projects = (await _repository.ListAsync())
                .Select(project => new ProjectDTO(project.Id, project.Name))
                .ToList();

            return Ok(response);
        }
    }
}
