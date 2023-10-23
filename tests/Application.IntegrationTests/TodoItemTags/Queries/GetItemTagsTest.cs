using FluentAssertions;
using NUnit.Framework;
using Todo_App.Application.TodoItemTags.Queries.GetTodos;

namespace Todo_App.Application.IntegrationTests.TodoItemTags.Queries;
using static Testing;


public class GetItemTagsTest : BaseTestFixture
{
    [Test]
    public async Task ShouldReturnAllItemTagDashboard()
    {
        await RunAsDefaultUserAsync();

        await AddAsync(new TodoItemTagDashboardDto
        {
            Title = "Title",
            Count = 99,
        });

        var query = new GetTodoItemTagsDashboardQuery();
        var result = await SendAsync(query);

        result.Should().HaveCount(99);
    }

    [Test]
    public async Task ShouldAcceptAnonymousUser()
    {
        var query = new GetTodoItemTagsDashboardQuery();
        var action = () => SendAsync(query);
        await action.Should().NotThrowAsync<UnauthorizedAccessException>();
    }
}
