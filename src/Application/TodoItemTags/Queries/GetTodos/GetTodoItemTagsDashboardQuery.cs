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
public record GetTodoItemTagsDashboardQuery: IRequest<IEnumerable<TodoItemTagDashboardDto>>;

public class GetTodoItemTagsDashboardQueryHandler : IRequestHandler<GetTodoItemTagsDashboardQuery, IEnumerable<TodoItemTagDashboardDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTodoItemTagsDashboardQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TodoItemTagDashboardDto>> Handle(GetTodoItemTagsDashboardQuery request, CancellationToken cancellationToken)
    {
        var x = await _context.TodoItemsTag
            .Where(w => w.IsDeleted != true)
            .GroupBy(g => g.Title)
            .Select(s => new TodoItemTagDashboardDto
            {
                Count = s.Count(),
                Title = s.Key
            })
            .OrderByDescending(o => o.Count)
            .ToListAsync();

        return x;
    }
}
