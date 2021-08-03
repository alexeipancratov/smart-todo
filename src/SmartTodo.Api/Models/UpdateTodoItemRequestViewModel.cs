using System;

namespace SmartTodo.Api.Models
{
    public class UpdateTodoItemRequestViewModel
    {
        public string Id { get; set; }
        
        public string Title { get; set; }

        public bool IsCompleted { get; set; }
    }
}