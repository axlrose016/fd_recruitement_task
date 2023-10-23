
using Todo_App.Application.Common.Mappings;
using Todo_App.Domain.Entities;
using Todo_App.Domain.ValueObjects;

namespace Todo_App.Application.TodoItemTags.Queries.GetTodos;
public class TodoItemTagDto : IMapFrom<TodoItemTag>
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public string? Title { get; set; }
    public string? Colour { get; set; }
    public bool IsDeleted { get; set; }
    public TodoItem Item { get; set; } = null!;
}
