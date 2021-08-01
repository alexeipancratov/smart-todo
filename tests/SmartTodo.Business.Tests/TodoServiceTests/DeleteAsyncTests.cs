using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SmartTodo.Data;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using SmartTodo.Business.Models;
using SmartTodo.Domain;

namespace SmartTodo.Business.Tests.TodoServiceTests
{
    [TestFixture]
    public class DeleteAsyncTests
    {
        private TodoService _todoService;
        private Mock<ILogger<TodoService>> _todoILoggerMock;
        private Mock<IValidator<CreateTodoItemRequest>> _createValidatorMock;
        private Mock<IValidator<UpdateTodoItemRequest>> _updateValidatorMock;
        private SmartTodoDbContext _dbContext;

        private readonly TodoItem _existingTodoItem = new TodoItem
        {
            Id = Guid.NewGuid().ToString(),
            Title = "Task 1",
            DateTimeCreated = DateTime.Now,
            IsCompleted = false,
            DateTimeCompleted = null
        };

        [SetUp]
        public async Task Setup()
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase("todos");
            _dbContext = new SmartTodoDbContext(builder.Options);
            await _dbContext.Database.EnsureDeletedAsync();

            _dbContext.TodoItems.Add(_existingTodoItem);
            await _dbContext.SaveChangesAsync();
            _dbContext.Entry(_existingTodoItem).State = EntityState.Detached;

            _todoILoggerMock = new Mock<ILogger<TodoService>>();
            _createValidatorMock = new Mock<IValidator<CreateTodoItemRequest>>();
            _updateValidatorMock = new Mock<IValidator<UpdateTodoItemRequest>>();

            _todoService = new TodoService(
                _todoILoggerMock.Object,
                _dbContext,
                _createValidatorMock.Object,
                _updateValidatorMock.Object);
        }

        [TearDown]
        public ValueTask CleanUp()
        {
            return _dbContext.DisposeAsync();
        }

        [Test]
        public async Task GivenNonExistingId_ShouldReturnResponseWithAnError()
        {
            var operationResponse = await _todoService.DeleteAsync("invalid_id_123");
            
            // Assert
            Assert.IsFalse(operationResponse.IsValid);
            Assert.AreEqual(1, operationResponse.Errors.Count());
        }
        
        [Test]
        public async Task GivenIdOfAnExistingTodoItem_ShouldRemoveItFromDatabase()
        {
            var operationResponse = await _todoService.DeleteAsync(_existingTodoItem.Id);
            
            // Assert
            Assert.IsTrue(operationResponse.IsValid);
            Assert.AreEqual(_existingTodoItem.Id, operationResponse.Result);
            var itemsCount = await _dbContext.TodoItems.CountAsync();
            Assert.AreEqual(0, itemsCount);
        }
    }
}
