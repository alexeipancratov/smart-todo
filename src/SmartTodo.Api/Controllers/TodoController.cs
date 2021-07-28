using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartTodo.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartTodo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ILogger<TodoController> _logger;

        public TodoController(ILogger<TodoController> logger)
        {
            _logger = logger;
        }

        [HttpGet("todos")]
        public IEnumerable<TodoItem> Get()
        {
            return new List<TodoItem>
            {
                new TodoItem { Id = Guid.NewGuid().ToString(), Title = "Wash dishes" }
            };
        }

        [HttpGet("todo")]
        public TodoItem Get(string id)
        {
            var items = new List<TodoItem>
            {
                new TodoItem { Id = Guid.NewGuid().ToString(), Title = "Wash dishes" }
            };

            return items.FirstOrDefault(i => string.Equals(i.Id, id, StringComparison.OrdinalIgnoreCase));
        }
    }
}
