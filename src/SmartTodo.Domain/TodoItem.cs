using System;

namespace SmartTodo.Domain
{
    public class TodoItem
    {
        public TodoItem()
        {}

        public TodoItem(string id, string title, DateTime dateTimeCreated)
        {
            Id = id;
            Title = title;
            DateTimeCreated = dateTimeCreated;
        }
        
        public string Id { get; set; }

        public string Title { get; set; }

        public DateTime DateTimeCreated { get; set; }

        public DateTime? DateTimeCompleted { get; set; }

        public bool IsCompleted { get; set; }
    }
}
