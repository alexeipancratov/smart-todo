using Microsoft.EntityFrameworkCore;
using SmartTodo.Domain;
using System;

namespace SmartTodo.Data
{
    public class SmartTodoDbContext : DbContext
    {
        public DbSet<TodoItem> TodoItems { get; set; }

        public string DbPath { get; set; }

        public SmartTodoDbContext()
        {
            SetupDbPath();
        }

        public SmartTodoDbContext(DbContextOptions opt)
            : base(opt)
        {
            SetupDbPath();
        }

        private void SetupDbPath()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);

            DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}smart-todo.db";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite($"Data Source={DbPath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var initialTodoItems = new TodoItem[]
            {
                new TodoItem
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Wash dishes",
                    DateTimeCreated = DateTime.Now,
                    DateTimeCompleted = null,
                    IsCompleted = false
                },
                new TodoItem
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Go shopping",
                    DateTimeCreated = DateTime.Now.AddDays(-1),
                    DateTimeCompleted = DateTime.Now.AddHours(-2),
                    IsCompleted = true
                },
                new TodoItem
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = "Meet friends",
                    DateTimeCreated = DateTime.Now.AddHours(5),
                    DateTimeCompleted = null,
                    IsCompleted = false
                },
            };
            modelBuilder.Entity<TodoItem>().HasData(initialTodoItems);
        }
    }
}
