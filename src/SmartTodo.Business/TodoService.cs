using SmartTodo.Data;
using SmartTodo.Domain;
using System.Collections.Generic;
using System.Linq;

namespace SmartTodo.Business
{
    public class TodoService : ITodoService
    {
        private readonly SmartTodoDbContext _dbContext;

        public TodoService(SmartTodoDbContext smartTodoDbContext)
        {
            _dbContext = smartTodoDbContext;
        }

        public IEnumerable<TodoItem> GetTodoItems()
        {
            return _dbContext.TodoItems.ToList();
        }
    }
}
