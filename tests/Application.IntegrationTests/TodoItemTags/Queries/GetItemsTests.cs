using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Todo_App.Application.TodoItemTags.Queries.GetTodos;
using Todo_App.Domain.Entities;
using Todo_App.Domain.ValueObjects;

namespace Todo_App.Application.IntegrationTests.TodoItemTags.Queries;

using static Testing;

public class GetItemsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldReturnAllTags()
    {
        await RunAsDefaultUserAsync();

        await AddAsync(new TodoItemTag
        {
            Title = "Celebration",
            Colour = Colour.Red,
        });

        var query = new GetTodoItemTagsQuery();
        var result = await SendAsync(query);

        result.ItemTags.Should().HaveCount(1);
    }

    [Test]
    public async Task ShouldAcceptAnonymousUser()
    {
        var query = new GetTodoItemTagsQuery();

        var action = () => SendAsync(query);

        await action.Should().NotThrowAsync<UnauthorizedAccessException>();
    }
}
