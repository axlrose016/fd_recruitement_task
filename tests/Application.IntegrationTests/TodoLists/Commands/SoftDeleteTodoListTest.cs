
namespace Todo_App.Application.IntegrationTests.TodoLists.Commands;

using FluentAssertions;
using NUnit.Framework;
using Todo_App.Application.Common.Exceptions;
using Todo_App.Application.TodoLists.Commands.CreateTodoList;
using Todo_App.Application.TodoLists.Commands.SoftDeleteTodoList;
using Todo_App.Domain.Entities;
using static Testing;
public class SoftDeleteTodoListTest : BaseTestFixture
{
    [Test]

    public async Task ShouldRequireValidTodoListId()
    {
        var command = new SoftDeleteTodoListCommand(99);
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldSoftDeleteTodoList()
    {
        var listId = await SendAsync(new CreateTodoListCommand
        {
            Title = "New List"
        });

        await SendAsync(new SoftDeleteTodoListCommand(listId));

        var list = await FindAsync<TodoList>(listId);

        list.Should().BeNull();
    }
}
