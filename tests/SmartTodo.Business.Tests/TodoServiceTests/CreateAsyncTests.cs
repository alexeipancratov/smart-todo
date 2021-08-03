using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using FluentValidation.Results;
using Moq;
using SmartTodo.Business.Models;

namespace SmartTodo.Business.Tests.TodoServiceTests
{
    [TestFixture]
    public class CreateAsyncTests : BaseTodoServiceTests
    {
        [Test]
        public async Task GivenValidationErrors_ShouldReturnResponseWithErrors()
        {
            // Arrange
            CreateValidatorMock
                .Setup(m => m.ValidateAsync(It.IsAny<CreateTodoItemRequest>(), default))
                .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("Title", "Title is required") }));

            // Act
            var operationResponse = await TodoService.CreateAsync(new CreateTodoItemRequest());
            
            // Assert
            Assert.IsFalse(operationResponse.IsValid);
            Assert.AreEqual(1, operationResponse.Errors.Count());
        }

        [Test]
        public async Task GivenCorrectTodoItem_ShouldSaveSuccessfullyToDb()
        {
            // Arrange
            CreateValidatorMock
                .Setup(m => m.ValidateAsync(It.IsAny<CreateTodoItemRequest>(), default))
                .ReturnsAsync(new ValidationResult());

            // Act
            var operationResponse = await TodoService.CreateAsync(new CreateTodoItemRequest());

            // Assert
            Assert.IsTrue(operationResponse.IsValid);
            Assert.IsFalse(string.IsNullOrWhiteSpace(operationResponse.Result.Id));
            var count = await DbContext.TodoItems.CountAsync();
            Assert.AreEqual(1, count);
        }
        
        [Test]
        public async Task GivenCorrectTodoItem_ShouldReturnValidOperationResponse()
        {
            // Arrange
            var currentTime = new DateTime(2021, 8, 2);
            CreateValidatorMock
                .Setup(m => m.ValidateAsync(It.IsAny<CreateTodoItemRequest>(), default))
                .ReturnsAsync(new ValidationResult());
            TimeProviderMock.Setup(m => m.GetCurrentServerTime()).Returns(currentTime);

            // Act
            var operationResponse = await TodoService.CreateAsync(new CreateTodoItemRequest());

            // Assert
            Assert.IsTrue(operationResponse.IsValid);
            Assert.IsNotNull(operationResponse.Result);
            Assert.AreEqual(currentTime, operationResponse.Result.DateTimeCreated);
        }
    }
}
