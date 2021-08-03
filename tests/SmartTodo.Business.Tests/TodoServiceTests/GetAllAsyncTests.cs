using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using SmartTodo.Domain;

namespace SmartTodo.Business.Tests.TodoServiceTests
{
    [TestFixture]
    public class GetAllAsyncTests : BaseTodoServiceTests
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            DbContext.TodoItems.AddRange(
                new TodoItem { Id = Guid.NewGuid().ToString(), Title = "Task 1" },
                new TodoItem { Id = Guid.NewGuid().ToString(), Title = "Task 2" },
                new TodoItem { Id = Guid.NewGuid().ToString(), Title = "Task 3" });
            DbContext.SaveChanges();
        }

        [Test]
        public async Task GivenThreeRecordsInDb_ShouldReturnAllThreeRecords()
        {
            List<TodoItem> todoItems = await TodoService.GetAllAsync();
            
            Assert.AreEqual(3, todoItems.Count);
        }
    }
}