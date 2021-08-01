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

namespace SmartTodo.Business.Tests.TodoServiceTests
{
    [TestFixture]
    public class CreateAsyncTests
    {
        private TodoService _todoService;
        private Mock<ILogger<TodoService>> _todoILoggerMock;
        private Mock<IValidator<CreateTodoItemRequest>> _createValidatorMock;
        private Mock<IValidator<UpdateTodoItemRequest>> _updateValidatorMock;
        private SmartTodoDbContext _dbContext;

        [SetUp]
        public void Setup()
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase("todos");
            _dbContext = new SmartTodoDbContext(builder.Options);
            _dbContext.Database.EnsureDeleted();

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
        public async Task GivenValidationErrors_ShouldReturnResponseWithErrors()
        {
            // Arrange
            _createValidatorMock
                .Setup(m => m.ValidateAsync(It.IsAny<CreateTodoItemRequest>(), default))
                .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("Title", "Title is required") }));

            // Act
            var operationResponse = await _todoService.CreateAsync(new CreateTodoItemRequest());
            
            // Assert
            Assert.IsFalse(operationResponse.IsValid);
            Assert.AreEqual(1, operationResponse.Errors.Count());
        }

        [Test]
        public async Task GivenCorrectTodoItem_ShouldSaveSuccessfullyToDb()
        {
            // Arrange
            _createValidatorMock
                .Setup(m => m.ValidateAsync(It.IsAny<CreateTodoItemRequest>(), default))
                .ReturnsAsync(new ValidationResult());

            // Act
            var operationResponse = await _todoService.CreateAsync(new CreateTodoItemRequest());

            // Assert
            Assert.IsTrue(operationResponse.IsValid);
            Assert.IsFalse(string.IsNullOrWhiteSpace(operationResponse.Result.Id));
            var count = await _dbContext.TodoItems.CountAsync();
            Assert.AreEqual(1, count);
        }
        
        [Test]
        public async Task GivenCorrectTodoItem_ShouldReturnValidOperationResponse()
        {
            // Arrange
            _createValidatorMock
                .Setup(m => m.ValidateAsync(It.IsAny<CreateTodoItemRequest>(), default))
                .ReturnsAsync(new ValidationResult());

            // Act
            var operationResponse = await _todoService.CreateAsync(new CreateTodoItemRequest());

            // Assert
            Assert.IsTrue(operationResponse.IsValid);
            Assert.IsNotNull(operationResponse.Result);
        }
    }
}
