namespace Todo_App.Domain.Events;

public class TodoItemTagsCreatedEvent : BaseEvent
{
    public TodoItemTagsCreatedEvent(TodoItemTag item)
    {
        Item = item;
    }

    public TodoItemTag Item { get; }
}
