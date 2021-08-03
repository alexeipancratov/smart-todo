using System;
using System.Linq;
using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartTodo.Domain;

namespace SmartTodo.Business.Tests.TodoServiceTests
{
    [TestFixture]
    public class DeleteAsyncTests : BaseTodoServiceTests
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
        public async Task GivenNonExistingId_ShouldReturnResponseWithAnError()
        {
            var operationResponse = await TodoService.DeleteAsync("invalid_id_123");

            // Assert
            Assert.IsFalse(operationResponse.IsValid);
            Assert.AreEqual(1, operationResponse.Errors.Count());
        }

        [Test]
        public async Task GivenIdOfAnExistingTodoItem_ShouldRemoveItFromDatabase()
        {
            var operationResponse = await TodoService.DeleteAsync(_existingTodoItem.Id);

            // Assert
            Assert.IsTrue(operationResponse.IsValid);
            Assert.AreEqual(_existingTodoItem.Id, operationResponse.Result);
            var itemsCount = DbContext.TodoItems.Count();
            Assert.AreEqual(0, itemsCount);
        }
    }
}