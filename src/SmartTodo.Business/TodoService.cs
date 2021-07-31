using Microsoft.EntityFrameworkCore;
using SmartTodo.Data;
using SmartTodo.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SmartTodo.Business.Models;

namespace SmartTodo.Business
{
    public class TodoService : ITodoService
    {
        private readonly ILogger<TodoService> _logger;
        private readonly SmartTodoDbContext _dbContext;
        private readonly IValidator<CreateTodoItemRequest> _createTodoItemValidator;

        public TodoService(ILogger<TodoService> logger, SmartTodoDbContext dbContext, IValidator<CreateTodoItemRequest> createTodoItemValidator)
        {
            _logger = logger;
            _dbContext = dbContext;
            _createTodoItemValidator = createTodoItemValidator;
        }

        public async Task<OperationResponse<TodoItem>> CreateAsync(CreateTodoItemRequest createTodoItemRequest)
        {
            var result = await _createTodoItemValidator.ValidateAsync(createTodoItemRequest);
            if (!result.IsValid)
            {
                return new OperationResponse<TodoItem>(result.Errors.Select(e => e.ErrorMessage));
            }

            var todoItem = new TodoItem(Guid.NewGuid().ToString(), createTodoItemRequest.Title, createTodoItemRequest.DateTimeCreated);

            _dbContext.TodoItems.Add(todoItem);
            await _dbContext.SaveChangesAsync();
            
            _logger.LogInformation($"Created todo item with ID '${todoItem.Id}' and title '${todoItem.Title}'");

            return new OperationResponse<TodoItem>(todoItem);
        }

        public Task<List<TodoItem>> GetAllAsync()
        {
            return _dbContext.TodoItems.ToListAsync();
        }
    }
}
