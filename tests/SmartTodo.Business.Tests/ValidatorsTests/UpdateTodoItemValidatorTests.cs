using System;
using FluentValidation.Results;
using NUnit.Framework;
using SmartTodo.Business.Models;
using SmartTodo.Business.Validators;

namespace SmartTodo.Business.Tests.ValidatorsTests
{
    [TestFixture]
    public class UpdateTodoItemValidatorTests
    {
        private UpdateTodoItemValidator _validator;
        
        [SetUp]
        public void Setup()
        {
            _validator = new UpdateTodoItemValidator();
        }

        [Test]
        public void GivenItemWithEmptyTitle_ShouldReturnError()
        {
            // Arrange
            var request = new UpdateTodoItemRequest
            {
                Id = "123",
                Title = String.Empty
            };
            
            // Act
            ValidationResult result = _validator.Validate(request);
            
            Assert.IsFalse(result.IsValid);
            Assert.IsNotEmpty(result.Errors);
        }
        
        [Test]
        public void GivenItemWithEmptyId_ShouldReturnError()
        {
            // Arrange
            var request = new UpdateTodoItemRequest
            {
                Id = "",
                Title = "Some title"
            };
            
            // Act
            ValidationResult result = _validator.Validate(request);
            
            Assert.IsFalse(result.IsValid);
            Assert.IsNotEmpty(result.Errors);
        }
    }
}