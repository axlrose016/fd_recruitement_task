

using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using NUnit.Framework;
using Todo_App.Application.TodoItemTags.Commands.CreateTodoItemTag;
using Todo_App.Domain.Entities;
using Todo_App.Domain.ValueObjects;

namespace Todo_App.Application.IntegrationTests.TodoItemTags.Command;

using static Testing;

public class CreateTodoItemTagsTest : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateTodoItemTagCommand();
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireUniqueTitle()
    {
        await SendAsync(new CreateTodoItemTagCommand
        {
            Title = "PS5",
            Colour = Colour.White
        });

        var command = new CreateTodoItemTagCommand
        {
            Title = "PS5",
            Colour = Colour.White
        };

        await FluentActions.Invoking(() =>
        SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldCreateTodoItemTag()
    {
        var userId = await RunAsDefaultUserAsync();

        var command = new CreateTodoItemTagCommand
        {
            Title = "Tag",
            Colour = Colour.White
        };

        var id = await SendAsync(command);

        var tag = await FindAsync<TodoItemTag>(id);

        tag.Should().NotBeNull();
        tag!.Title.Should().Be(command.Title);
        tag.CreatedBy.Should().Be(userId);
        tag.Created.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
