namespace Todo_App.Domain.Entities;
public class TodoItemTag : BaseAuditableEntity
{
    public int ItemId { get; set; }
    public string? Title { get; set; }
    public Colour Colour { get; set; } = Colour.White;
    private bool _isdeleted;
    public bool IsDeleted
    {
        get => _isdeleted;
        set
        {
            if (value == true && _isdeleted == false)
            {
                AddDomainEvent(new TodoItemTagDeletedEvent(this));
            }

            _isdeleted = value;
        }
    }

    public TodoItem Item { get; set; } = null!;
}
