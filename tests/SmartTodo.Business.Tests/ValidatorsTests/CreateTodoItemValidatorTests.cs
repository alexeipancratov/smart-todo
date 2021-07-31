using System;
using FluentValidation.Results;
using NUnit.Framework;
using SmartTodo.Business.Models;
using SmartTodo.Business.Validators;

namespace SmartTodo.Business.Tests.ValidatorsTests
{
    [TestFixture]
    public class CreateTodoItemValidatorTests
    {
        private CreateTodoItemValidator _validator;
        
        [SetUp]
        public void Setup()
        {
            _validator = new CreateTodoItemValidator();
        }

        [Test]
        public void GivenItemWithEmptyTitle_ShouldReturnError()
        {
            // Arrange
            var request = new CreateTodoItemRequest
            {
                Title = String.Empty,
                DateTimeCreated = DateTime.Now
            };
            
            // Act
            ValidationResult result = _validator.Validate(request);
            
            Assert.IsFalse(result.IsValid);
            Assert.IsNotEmpty(result.Errors);
        }
        
        [Test]
        public void GivenItemWithMinValueDateTimeCreated_ShouldReturnError()
        {
            // Arrange
            var request = new CreateTodoItemRequest
            {
                Title = "Some title",
                DateTimeCreated = default
            };
            
            // Act
            ValidationResult result = _validator.Validate(request);
            
            Assert.IsFalse(result.IsValid);
            Assert.IsNotEmpty(result.Errors);
        }
    }
}