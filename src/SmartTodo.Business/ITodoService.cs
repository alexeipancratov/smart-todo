using SmartTodo.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartTodo.Business.Models;

namespace SmartTodo.Business
{
    public interface ITodoService
    {
        Task<OperationResponse<TodoItem>> CreateAsync(CreateTodoItemRequest todoItemRequest);

        Task<List<TodoItem>> GetAllAsync();

        Task<OperationResponse<TodoItem>> UpdateAsync(UpdateTodoItemRequest updateTodoItemRequest);
    }
}
