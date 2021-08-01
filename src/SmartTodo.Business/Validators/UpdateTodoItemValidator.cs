using FluentValidation;
using SmartTodo.Business.Models;

namespace SmartTodo.Business.Validators
{
    public class UpdateTodoItemValidator : AbstractValidator<UpdateTodoItemRequest>
    {
        public UpdateTodoItemValidator()
        {
            RuleFor(r => r.Id).NotEmpty();
            RuleFor(r => r.Title).NotEmpty();
        }
    }
}