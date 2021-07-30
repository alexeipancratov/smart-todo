using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartTodo.Business;
using SmartTodo.Domain;
using System.Collections.Generic;

namespace SmartTodo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ILogger<TodoController> _logger;
        private readonly ITodoService _todoService;

        public TodoController(ILogger<TodoController> logger, ITodoService todoService)
        {
            _logger = logger;
            _todoService = todoService;
        }

        [HttpGet("todos")]
        public IEnumerable<TodoItem> Get()
        {
            return _todoService.GetTodoItems();
        }
    }
}
