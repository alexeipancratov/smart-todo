using SmartTodo.Domain;
using System.Collections.Generic;

namespace SmartTodo.Business
{
    public interface ITodoService
    {
        IEnumerable<TodoItem> GetTodoItems();
    }
}
