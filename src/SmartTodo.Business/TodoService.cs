using Microsoft.EntityFrameworkCore;
using SmartTodo.Data;
using SmartTodo.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SmartTodo.Business.Infrastructure;
using SmartTodo.Business.Models;

namespace SmartTodo.Business
{
    public class TodoService : ITodoService
    {
        private readonly ILogger<TodoService> _logger;
        private readonly ITimeProvider _timeProvider;
        private readonly SmartTodoDbContext _dbContext;
        private readonly IValidator<CreateTodoItemRequest> _createValidator;
        private readonly IValidator<UpdateTodoItemRequest> _updateValidator;

        public TodoService(
            ILogger<TodoService> logger,
            ITimeProvider timeProvider,
            SmartTodoDbContext dbContext,
            IValidator<CreateTodoItemRequest> createValidator,
            IValidator<UpdateTodoItemRequest> updateValidator)
        {
            _logger = logger;
            _timeProvider = timeProvider;
            _dbContext = dbContext;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<OperationResponse<TodoItem>> CreateAsync(CreateTodoItemRequest createTodoItemRequest)
        {
            var validationResult = await _createValidator.ValidateAsync(createTodoItemRequest);
            if (!validationResult.IsValid)
            {
                return new OperationResponse<TodoItem>(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            var todoItem = new TodoItem(Guid.NewGuid().ToString(), createTodoItemRequest.Title, _timeProvider.GetCurrentServerTime());

            _dbContext.TodoItems.Add(todoItem);
            await _dbContext.SaveChangesAsync();
            
            _logger.LogInformation("Created todo item with ID '{ID}' and title '{Title}'", todoItem.Id, todoItem.Title);

            return new OperationResponse<TodoItem>(todoItem);
        }

        public Task<List<TodoItem>> GetAllAsync()
        {
            return _dbContext.TodoItems.Take(100).ToListAsync();
        }

        public async Task<OperationResponse<TodoItem>> UpdateAsync(UpdateTodoItemRequest updateTodoItemRequest)
        {
            var validationResult = await _updateValidator.ValidateAsync(updateTodoItemRequest);
            if (!validationResult.IsValid)
            {
                return new OperationResponse<TodoItem>(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            TodoItem todoItem = await _dbContext.TodoItems.FindAsync(updateTodoItemRequest.Id);

            if (todoItem == null)
            {
                return new OperationResponse<TodoItem>(new [] {"Todo item not found."});
            }
            
            if (!todoItem.IsCompleted && updateTodoItemRequest.IsCompleted)
            {
                todoItem.DateTimeCompleted = _timeProvider.GetCurrentServerTime();
                _logger.LogInformation("Marked todo item with ID '{ID}' as completed", todoItem.Id);
            }
            todoItem.Title = updateTodoItemRequest.Title;
            todoItem.IsCompleted = updateTodoItemRequest.IsCompleted;

            _dbContext.TodoItems.Update(todoItem);
            await _dbContext.SaveChangesAsync();
            
            _logger.LogInformation("Updated todo item with ID '{ID}'", todoItem.Id);

            return new OperationResponse<TodoItem>(todoItem);
        }

        public async Task<OperationResponse<string>> DeleteAsync(string id)
        {
            TodoItem todoItem = await _dbContext.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return new OperationResponse<string>(new[] {"Todo item not found."});
            }
            
            _dbContext.Remove(todoItem);
            await _dbContext.SaveChangesAsync();
            
            _logger.LogInformation("Deleted todo item with ID '{ID}' and title '{Title}'", todoItem.Id, todoItem.Title);

            return new OperationResponse<string>(id);
        }
    }
}
