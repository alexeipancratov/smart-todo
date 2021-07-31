using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SmartTodo.Business.Models;
using SmartTodo.Data;
using SmartTodo.Domain;

namespace SmartTodo.Business.Tests.TodoServiceTests
{
    [TestFixture]
    public class GetAllAsyncTests
    {
        private TodoService _todoService;
        private Mock<ILogger<TodoService>> _todoILoggerMock;
        private Mock<IValidator<CreateTodoItemRequest>> _createValidatorMock;
        private SmartTodoDbContext _dbContext;

        [SetUp]
        public async Task Setup()
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase("todos");
            _dbContext = new SmartTodoDbContext(builder.Options);
            await _dbContext.Database.EnsureDeletedAsync();
            
            _dbContext.TodoItems.AddRange(
                new TodoItem { Id = Guid.NewGuid().ToString(), Title = "Task 1" },
                new TodoItem { Id = Guid.NewGuid().ToString(), Title = "Task 2" },
                new TodoItem { Id = Guid.NewGuid().ToString(), Title = "Task 3" });
            await _dbContext.SaveChangesAsync();

            _todoILoggerMock = new Mock<ILogger<TodoService>>();
            _createValidatorMock = new Mock<IValidator<CreateTodoItemRequest>>();

            _todoService = new TodoService(_todoILoggerMock.Object, _dbContext, _createValidatorMock.Object);
        }

        [TearDown]
        public ValueTask CleanUp()
        {
            return _dbContext.DisposeAsync();
        }

        [Test]
        public async Task GivenThreeRecordsInDb_ShouldReturnAllThreeRecords()
        {
            List<TodoItem> todoItems = await _todoService.GetAllAsync();
            
            Assert.AreEqual(3, todoItems.Count);
        }
    }
}