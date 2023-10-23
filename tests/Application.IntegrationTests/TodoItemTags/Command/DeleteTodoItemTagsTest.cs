
namespace Todo_App.Application.IntegrationTests.TodoItemTags.Command;

using FluentAssertions;
using NUnit.Framework;
using Todo_App.Application.Common.Exceptions;
using Todo_App.Application.TodoItems.Commands.CreateTodoItem;
using Todo_App.Application.TodoItemTags.Commands.CreateTodoItemTag;
using Todo_App.Application.TodoItemTags.Commands.DeleteTodoItemTag;
using Todo_App.Domain.Entities;
using Todo_App.Domain.ValueObjects;
using static Testing;
public class DeleteTodoItemTagsTest : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidTodoItemId()
    {
        var command = new DeleteTodoItemTagCommand(99);
        await FluentActions.Invoking(() =>
        SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldDeleteTodoItemTag()
    {
        var itemId = await SendAsync(new CreateTodoItemCommand
        {
            Title = "New Item"
        });

        var tagId = await SendAsync(new CreateTodoItemTagCommand
        {
            ItemId = itemId,
            Title = "New Tag",
            Colour = Colour.White
        });

        await SendAsync(new DeleteTodoItemTagCommand(tagId));
        var tag = await FindAsync<TodoItemTag>(tagId);

        tag.Should().BeNull();
    }
}
