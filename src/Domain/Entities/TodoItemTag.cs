namespace Todo_App.Domain.Entities;
public class TodoItemTag : BaseAuditableEntity
{
    public int ItemId { get; set; }
    public string? Title { get; set; }
    public Colour Colour { get; set; } = Colour.White;
    public TodoItem Item { get; set; } = null!;
}
