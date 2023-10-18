using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo_App.Application.Common.Interfaces;

namespace Todo_App.Application.TodoItemTags.Queries.GetTodos;
public record GetTodoItemTagsQuery : IRequest<TodoItemTagVm>;

public class GetTodoItemTagsQueryHandler : IRequestHandler<GetTodoItemTagsQuery, TodoItemTagVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTodoItemTagsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TodoItemTagVm> Handle(GetTodoItemTagsQuery request, CancellationToken cancellationToken)
    {
        return new TodoItemTagVm
        {
            ItemTags = await _context.TodoItemsTag
            .AsNoTracking()
            .ProjectTo<TodoItemTagDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken)
        };
    }
}
