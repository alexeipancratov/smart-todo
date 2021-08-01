using System;
using Microsoft.AspNetCore.Mvc;
using SmartTodo.Business;
using SmartTodo.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartTodo.Api.Models;
using SmartTodo.Business.Models;

namespace SmartTodo.Api.Controllers
{
    [Route("api/todos")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodosController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            IEnumerable<TodoItem> todos = await _todoService.GetAllAsync();

            return Ok(todos);
        }

        [HttpPost]
        public async Task<ActionResult> Post(CreateTodoItemRequestViewModel todoItem)
        {
            var createRequest = new CreateTodoItemRequest
            {
                Title = todoItem.Title,
                DateTimeCreated = DateTime.Now
            };
            var operationResponse = await _todoService.CreateAsync(createRequest);

            if (operationResponse.IsValid)
            {
                return Ok(operationResponse.Result);
            }

            return BadRequest(operationResponse.Errors);
        }

        [HttpPut]
        public async Task<ActionResult> Put(UpdateTodoItemRequestViewModel todoItem)
        {
            var updateRequest = new UpdateTodoItemRequest
            {
                Id = todoItem.Id,
                Title = todoItem.Title,
                DateTimeCompleted = todoItem.DateTimeCompleted,
                IsCompleted = todoItem.IsCompleted
            };
            var operationResponse = await _todoService.UpdateAsync(updateRequest);

            if (operationResponse.IsValid)
            {
                return Ok(operationResponse.Result);
            }

            return BadRequest(operationResponse.Errors);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var operationResponse = await _todoService.DeleteAsync(id);

            if (operationResponse.IsValid)
            {
                return Ok(operationResponse.Result);
            }

            return BadRequest(operationResponse.Errors);
        }
    }
}
