using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Todo_App.Application.Common.Interfaces;

namespace Todo_App.Application.TodoItemTags.Commands.CreateTodoItemTag;
internal class CreateTodoItemTagCommandValidator : AbstractValidator<CreateTodoItemTagCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateTodoItemTagCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("Tag is Required")
            .MaximumLength(200).WithMessage("Tag must not exceed 200 characters.");
    }
}
