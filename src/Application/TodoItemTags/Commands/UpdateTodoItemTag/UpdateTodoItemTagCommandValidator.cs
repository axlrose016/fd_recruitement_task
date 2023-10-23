using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Todo_App.Application.Common.Interfaces;

namespace Todo_App.Application.TodoItemTags.Commands.UpdateTodoItemTag;
public class UpdateTodoItemTagCommandValidator : AbstractValidator<UpdateTodoItemTagCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTodoItemTagCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.TagDescription)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");
    }
}
