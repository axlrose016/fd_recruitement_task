

namespace Todo_App.Application.IntegrationTests.TodoItemTags.Command;

using System.Drawing;
using System.Runtime.InteropServices;
using FluentAssertions;
using NUnit.Framework;
using Todo_App.Application.Common.Exceptions;
using Todo_App.Application.TodoItemTags.Commands.CreateTodoItemTag;
using Todo_App.Application.TodoItemTags.Commands.UpdateTodoItemTag;
using Todo_App.Domain.Entities;
using Todo_App.Domain.ValueObjects;
using static Testing;

public class UpdateTodoItemTagsTest : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidTodoItemTagId()
    {
        var command = new UpdateTodoItemTagCommand
        {
            Id = 99,
            TagDescription = "New Tag",
            Colour = Colour.Blue
        };
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldUpdatedTodoItemTag()
    {
        var userId = await RunAsDefaultUserAsync();

        var tagId = await SendAsync(new CreateTodoItemTagCommand
        {
            Title = "New Tag",
            Colour = Colour.White
        });

        var command = new UpdateTodoItemTagCommand
        {
            Id = tagId,
            TagDescription = "Updated Tag Description"
        };

        await SendAsync(command);

        var item = await FindAsync<TodoItemTag>(tagId);

        item.Should().NotBeNull();
        item!.Title.Should().Be(command.TagDescription);
        item.LastModifiedBy.Should().NotBeNull();
        item.LastModifiedBy.Should().Be(userId);
        item.LastModified.Should().NotBeNull();
        item.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));

    }


}
