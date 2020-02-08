using CleanArchitecture.Core.Entities;
using CleanArchitecture.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CleanArchitecture.Web.Endpoints.ToDoItems
{
    public class ListEndpoint : BaseEndpoint<List<ToDoItemResult>>
    {
        private readonly IRepository _repository;

        public ListEndpoint(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("/endpoints/items")]
        public override ActionResult<List<ToDoItemResult>> Handle()
        {
            var result = _repository.List<ToDoItem>()
                .Select(i => new ToDoItemResult
                {
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.Description
                });
            return Ok(result);
        }
    }

    // Imagine these are separate classes attached to the Handler/Endpoint with + expansion in VS
    public class ToDoItemResult
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

}
