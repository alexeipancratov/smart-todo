using System;
using FluentValidation;
using SmartTodo.Business.Models;

namespace SmartTodo.Business.Validators
{
    public class CreateTodoItemValidator : AbstractValidator<CreateTodoItemRequest>
    {
        public CreateTodoItemValidator()
        {
            RuleFor(t => t.Title).NotEmpty();
            RuleFor(t => t.DateTimeCreated).NotEqual(DateTime.MinValue);
        }
    }
}