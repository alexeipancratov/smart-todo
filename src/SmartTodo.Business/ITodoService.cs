using SmartTodo.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartTodo.Business.Models;

namespace SmartTodo.Business
{
    public interface ITodoService
    {
        /// <summary>
        /// Creates a To Do Item and saves it in a database.
        /// </summary>
        /// <param name="todoItemRequest">Request containing data to create a To Do Item from.</param>
        /// <returns>Operation Response containing result or errors.</returns>
        Task<OperationResponse<TodoItem>> CreateAsync(CreateTodoItemRequest todoItemRequest);

        /// <summary>
        /// Retrieves all To Do Items from database.
        /// </summary>
        /// <returns>Collection of all To Do Items.</returns>
        Task<List<TodoItem>> GetAllAsync();

        /// <summary>
        /// Updates a To Do Item in a database.
        /// </summary>
        /// <param name="updateTodoItemRequest">Request containing data to update a To Do Item from.</param>
        /// <returns>Operation Response containing result or errors.</returns>
        Task<OperationResponse<TodoItem>> UpdateAsync(UpdateTodoItemRequest updateTodoItemRequest);

        /// <summary>
        /// Deletes a To Do Item with the specified identifier.
        /// </summary>
        /// <param name="id">ID of a To Do Item to delete.</param>
        /// <returns>ID of a deleted To Do Item.</returns>
        Task<OperationResponse<string>> DeleteAsync(string id);
    }
}
