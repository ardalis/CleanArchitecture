using CleanArchitecture.Core.Entities;
using CleanArchitecture.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.Web.Endpoints.ToDoItems
{
    public class GetById : Ardalis.ApiEndpoints.BaseAsyncEndpoint<GetByIdRequest,ToDoItemResponse>
    {
        private readonly IRepository _repository;

        public GetById(IRepository repository)
        {
            _repository = repository;
        }
        [HttpGet("{id:int}")]
        public override async Task<ActionResult<ToDoItemResponse>> HandleAsync(GetByIdRequest request)
        {
            var item = await _repository.GetByIdAsync<ToDoItem>(request.Id);

            var response = new ToDoItemResponse
            {
                Id = item.Id,
                Description = item.Description,
                IsDone = item.IsDone,
                Title = item.Title
            };
            return Ok(response);
        }
    }
}
