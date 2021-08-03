using System;

namespace SmartTodo.Business.Models
{
    public class UpdateTodoItemRequest
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public bool IsCompleted { get; set; }
    }
}