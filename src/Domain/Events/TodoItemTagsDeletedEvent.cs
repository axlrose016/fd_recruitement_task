namespace Todo_App.Domain.Events;

public class TodoItemTagDeletedEvent : BaseEvent
{
    public TodoItemTagDeletedEvent(TodoItemTag item)
    {
        Item = item;
    }

    public TodoItemTag Item { get; set; }
}
