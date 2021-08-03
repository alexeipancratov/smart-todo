using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using FluentValidation.Results;
using Moq;
using SmartTodo.Business.Models;
using SmartTodo.Domain;

namespace SmartTodo.Business.Tests.TodoServiceTests
{
    [TestFixture]
    public class UpdateAsyncTests : BaseTodoServiceTests
    {
        private readonly TodoItem _existingTodoItem = new TodoItem
        {
            Id = Guid.NewGuid().ToString(),
            Title = "Task 1",
            DateTimeCreated = DateTime.Now,
            IsCompleted = false,
            DateTimeCompleted = null
        };

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            DbContext.TodoItems.Add(_existingTodoItem);
            DbContext.SaveChanges();
            DbContext.Entry(_existingTodoItem).State = EntityState.Detached;
        }

        [Test]
        public async Task GivenValidationErrors_ShouldReturnResponseWithAnError()
        {
            // Arrange
            UpdateValidatorMock
                .Setup(m => m.ValidateAsync(It.IsAny<UpdateTodoItemRequest>(), default))
                .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("Title", "Title is required") }));

            // Act
            var operationResponse = await TodoService.UpdateAsync(new UpdateTodoItemRequest());
            
            // Assert
            Assert.IsFalse(operationResponse.IsValid);
            Assert.AreEqual(1, operationResponse.Errors.Count());
        }
        
        [Test]
        public async Task GivenValidTodoItemWithNonExistingId_ShouldReturnResponseWithErrors()
        {
            // Arrange
            UpdateValidatorMock
                .Setup(m => m.ValidateAsync(It.IsAny<UpdateTodoItemRequest>(), default))
                .ReturnsAsync(new ValidationResult());
            
            var updateRequest = new UpdateTodoItemRequest
            {
                Id = Guid.NewGuid().ToString(),
                Title = _existingTodoItem.Title,
                IsCompleted = true
            };

            // Act
            var operationResponse = await TodoService.UpdateAsync(updateRequest);
            
            // Assert
            Assert.IsFalse(operationResponse.IsValid);
            Assert.AreEqual(1, operationResponse.Errors.Count());
        }
        
        [Test]
        public async Task GivenCorrectTodoItem_ShouldUpdateInDbAndReturnFullyUpdatedItem()
        {
            // Arrange
            UpdateValidatorMock
                .Setup(m => m.ValidateAsync(It.IsAny<UpdateTodoItemRequest>(), default))
                .ReturnsAsync(new ValidationResult());

            var updateRequest = new UpdateTodoItemRequest
            {
                Id = _existingTodoItem.Id,
                Title = _existingTodoItem.Title,
                IsCompleted = true
            };

            // Act
            var operationResponse = await TodoService.UpdateAsync(updateRequest);

            // Assert
            Assert.IsTrue(operationResponse.IsValid);
            var updatedTodoItem = await DbContext.TodoItems.FindAsync(_existingTodoItem.Id);
            Assert.AreEqual(updateRequest.IsCompleted, updatedTodoItem.IsCompleted);
            Assert.AreNotEqual(default(DateTime), updatedTodoItem.DateTimeCreated);
        }
        
        [Test]
        public async Task GivenCorrectTodoItem_ShouldReturnUpdatedTodoItem()
        {
            // Arrange
            UpdateValidatorMock
                .Setup(m => m.ValidateAsync(It.IsAny<UpdateTodoItemRequest>(), default))
                .ReturnsAsync(new ValidationResult());
                
            var updateRequest = new UpdateTodoItemRequest
            {
                Id = _existingTodoItem.Id,
                Title = _existingTodoItem.Title,
                IsCompleted = true
            };

            // Act
            var operationResponse = await TodoService.UpdateAsync(updateRequest);

            // Assert
            Assert.IsTrue(operationResponse.IsValid);
            Assert.AreEqual(updateRequest.IsCompleted, operationResponse.Result.IsCompleted);
        }
        
        [Test]
        public async Task GivenCompletedTodoItemWhichWasNotCompletedBefore_ShouldSetCorrectDateTimeCompleted()
        {
            // Arrange
            var currentTime = new DateTime(2021, 8, 2);
            UpdateValidatorMock
                .Setup(m => m.ValidateAsync(It.IsAny<UpdateTodoItemRequest>(), default))
                .ReturnsAsync(new ValidationResult());
            TimeProviderMock.Setup(m => m.GetCurrentServerTime()).Returns(currentTime);
                
            var updateRequest = new UpdateTodoItemRequest
            {
                Id = _existingTodoItem.Id,
                Title = _existingTodoItem.Title,
                IsCompleted = true
            };

            // Act
            var operationResponse = await TodoService.UpdateAsync(updateRequest);

            // Assert
            Assert.AreEqual(currentTime, operationResponse.Result.DateTimeCompleted);
        }
    }
}
