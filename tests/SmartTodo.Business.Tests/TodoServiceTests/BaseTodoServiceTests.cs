using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SmartTodo.Business.Infrastructure;
using SmartTodo.Business.Models;
using SmartTodo.Data;

namespace SmartTodo.Business.Tests.TodoServiceTests
{
    public abstract class BaseTodoServiceTests
    {
        protected TodoService TodoService;
        private Mock<ILogger<TodoService>> TodoILoggerMock;
        protected Mock<ITimeProvider> TimeProviderMock;
        protected Mock<IValidator<CreateTodoItemRequest>> CreateValidatorMock;
        protected Mock<IValidator<UpdateTodoItemRequest>> UpdateValidatorMock;
        protected SmartTodoDbContext DbContext;
        
        [SetUp]
        public virtual void SetUp()
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase("todos");
            DbContext = new SmartTodoDbContext(builder.Options);
            DbContext.Database.EnsureDeleted();

            TodoILoggerMock = new Mock<ILogger<TodoService>>();
            TimeProviderMock = new Mock<ITimeProvider>();
            CreateValidatorMock = new Mock<IValidator<CreateTodoItemRequest>>();
            UpdateValidatorMock = new Mock<IValidator<UpdateTodoItemRequest>>();

            TodoService = new TodoService(
                TodoILoggerMock.Object,
                TimeProviderMock.Object,
                DbContext,
                CreateValidatorMock.Object,
                UpdateValidatorMock.Object);
        }
        
        [TearDown]
        public ValueTask CleanUp()
        {
            return DbContext.DisposeAsync();
        }
    }
}