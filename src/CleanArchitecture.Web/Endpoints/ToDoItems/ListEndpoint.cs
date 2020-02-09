using CleanArchitecture.Core.Entities;
using CleanArchitecture.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CleanArchitecture.Web.Endpoints.ToDoItems
{
    public class ListEndpoint : BaseEndpoint<List<ListItemResult>>
    {
        private readonly IRepository _repository;

        public ListEndpoint(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("/endpoints/items")]
        public override ActionResult<List<ListItemResult>> Handle()
        {
            var result = _repository.List<ToDoItem>()
                .Select(i => new ListItemResult
                {
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.Description
                });
            return Ok(result);
        }
    }
}
