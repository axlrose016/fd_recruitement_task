using MediatR;
using Todo_App.Application.Common.Interfaces;
using Todo_App.Domain.Entities;
using Todo_App.Domain.ValueObjects;

namespace Todo_App.Application.TodoItemTags.Commands.CreateTodoItemTag;
public class CreateTodoItemTagCommand : IRequest<int>
{
    public int ItemId { get; init; }
    public string? Title { get; init; }
    public string? Colour { get; init; }
}

public class CreateTodoItemTagCommandHandler : IRequestHandler<CreateTodoItemTagCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateTodoItemTagCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateTodoItemTagCommand request, CancellationToken cancellationToken)
    {
        var entity = new TodoItemTag();
        entity.ItemId = request.ItemId;
        entity.Title = request.Title;
        entity.Colour = Colour.From(request.Colour.ToUpper());
        _context.TodoItemsTag.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
