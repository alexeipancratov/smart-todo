using System;

namespace SmartTodo.Business.Models
{
    public class CreateTodoItemRequest
    {
        public string Title { get; init; }

        public DateTime DateTimeCreated { get; init; }
    }
}