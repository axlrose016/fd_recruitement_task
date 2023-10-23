using MediatR;
using Todo_App.Application.Common.Exceptions;
using Todo_App.Application.Common.Interfaces;
using Todo_App.Application.TodoLists.Commands.UpdateTodoList;
using Todo_App.Domain.Entities;
using Todo_App.Domain.ValueObjects;

namespace Todo_App.Application.TodoItemTags.Commands.UpdateTodoItemTag;
public class UpdateTodoItemTagCommand : IRequest
{
    public int Id { get; init; }
    public string? TagDescription { get; init; }
    public string? Colour { get; init; }
}

public class UpdateTodoItemTagCommandHandler: IRequestHandler<UpdateTodoItemTagCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTodoItemTagCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateTodoItemTagCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoLists
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(TodoItemTag), request.Id);
        }

        entity.Title = request.TagDescription;
        entity.Colour = Colour.From(request.Colour.ToUpper());

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
