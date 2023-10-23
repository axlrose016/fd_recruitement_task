
using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo_App.Application.Common.Exceptions;
using Todo_App.Application.Common.Interfaces;

namespace Todo_App.Application.TodoItems.Commands.SoftDeleteTodoItem;

public record SoftDeleteTodoItemCommand(int Id) : IRequest;

public class SoftDeleteTodoItemCommandHandler : IRequestHandler<SoftDeleteTodoItemCommand>
{
    private readonly IApplicationDbContext _context;

    public SoftDeleteTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(SoftDeleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoItems
            .Include(i => i.Tags)
            .Where(w => w.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if(entity == null)
        {
            throw new NotFoundException(nameof(TodoItems), request.Id);
        }

        if(entity.Tags.Count > 0)
        {
            foreach(var tag in entity.Tags)
            {
                tag.IsDeleted = true;
                _context.TodoItemsTag.Update(tag);
            }
        }

        entity.IsDeleted = true;
        _context.TodoItems.Update(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
