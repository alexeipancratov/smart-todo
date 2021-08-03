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
using SmartTodo.Business.Infrastructure;
using SmartTodo.Business.Models;
using SmartTodo.Domain;

namespace SmartTodo.Business.Tests.TodoServiceTests
{
    [TestFixture]
    public class UpdateAsyncTests
    {
        private TodoService _todoService;
        private Mock<ILogger<TodoService>> _todoILoggerMock;
        private Mock<ITimeProvider> _timeProviderMock;
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
            _timeProviderMock = new Mock<ITimeProvider>();
            _createValidatorMock = new Mock<IValidator<CreateTodoItemRequest>>();
            _updateValidatorMock = new Mock<IValidator<UpdateTodoItemRequest>>();

            _todoService = new TodoService(
                _todoILoggerMock.Object,
                _timeProviderMock.Object,
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
        public async Task GivenValidationErrors_ShouldReturnResponseWithAnError()
        {
            // Arrange
            _updateValidatorMock
                .Setup(m => m.ValidateAsync(It.IsAny<UpdateTodoItemRequest>(), default))
                .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("Title", "Title is required") }));

            // Act
            var operationResponse = await _todoService.UpdateAsync(new UpdateTodoItemRequest());
            
            // Assert
            Assert.IsFalse(operationResponse.IsValid);
            Assert.AreEqual(1, operationResponse.Errors.Count());
        }
        
        [Test]
        public async Task GivenValidTodoItemWithNonExistingId_ShouldReturnResponseWithErrors()
        {
            // Arrange
            _updateValidatorMock
                .Setup(m => m.ValidateAsync(It.IsAny<UpdateTodoItemRequest>(), default))
                .ReturnsAsync(new ValidationResult());
            
            var updateRequest = new UpdateTodoItemRequest
            {
                Id = Guid.NewGuid().ToString(),
                Title = _existingTodoItem.Title,
                IsCompleted = true
            };

            // Act
            var operationResponse = await _todoService.UpdateAsync(updateRequest);
            
            // Assert
            Assert.IsFalse(operationResponse.IsValid);
            Assert.AreEqual(1, operationResponse.Errors.Count());
        }
        
        [Test]
        public async Task GivenCorrectTodoItem_ShouldUpdateInDbAndReturnFullyUpdatedItem()
        {
            // Arrange
            _updateValidatorMock
                .Setup(m => m.ValidateAsync(It.IsAny<UpdateTodoItemRequest>(), default))
                .ReturnsAsync(new ValidationResult());

            var updateRequest = new UpdateTodoItemRequest
            {
                Id = _existingTodoItem.Id,
                Title = _existingTodoItem.Title,
                IsCompleted = true
            };

            // Act
            var operationResponse = await _todoService.UpdateAsync(updateRequest);

            // Assert
            Assert.IsTrue(operationResponse.IsValid);
            var updatedTodoItem = await _dbContext.TodoItems.FindAsync(_existingTodoItem.Id);
            Assert.AreEqual(updateRequest.IsCompleted, updatedTodoItem.IsCompleted);
            Assert.AreNotEqual(default(DateTime), updatedTodoItem.DateTimeCreated);
        }
        
        [Test]
        public async Task GivenCorrectTodoItem_ShouldReturnUpdatedTodoItem()
        {
            // Arrange
            _updateValidatorMock
                .Setup(m => m.ValidateAsync(It.IsAny<UpdateTodoItemRequest>(), default))
                .ReturnsAsync(new ValidationResult());
                
            var updateRequest = new UpdateTodoItemRequest
            {
                Id = _existingTodoItem.Id,
                Title = _existingTodoItem.Title,
                IsCompleted = true
            };

            // Act
            var operationResponse = await _todoService.UpdateAsync(updateRequest);

            // Assert
            Assert.IsTrue(operationResponse.IsValid);
            Assert.AreEqual(updateRequest.IsCompleted, operationResponse.Result.IsCompleted);
        }
        
        [Test]
        public async Task GivenCompletedTodoItemWhichWasNotCompletedBefore_ShouldSetCorrectDateTimeCompleted()
        {
            // Arrange
            var currentTime = new DateTime(2021, 8, 2);
            _updateValidatorMock
                .Setup(m => m.ValidateAsync(It.IsAny<UpdateTodoItemRequest>(), default))
                .ReturnsAsync(new ValidationResult());
            _timeProviderMock.Setup(m => m.GetCurrentServerTime()).Returns(currentTime);
                
            var updateRequest = new UpdateTodoItemRequest
            {
                Id = _existingTodoItem.Id,
                Title = _existingTodoItem.Title,
                IsCompleted = true
            };

            // Act
            var operationResponse = await _todoService.UpdateAsync(updateRequest);

            // Assert
            Assert.AreEqual(currentTime, operationResponse.Result.DateTimeCompleted);
        }
    }
}
