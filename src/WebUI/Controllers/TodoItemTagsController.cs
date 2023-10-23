using Microsoft.AspNetCore.Mvc;
using Todo_App.Application.TodoItemTags.Commands.CreateTodoItemTag;
using Todo_App.Application.TodoItemTags.Commands.DeleteTodoItemTag;
using Todo_App.Application.TodoItemTags.Commands.UpdateTodoItemTag;
using Todo_App.Application.TodoItemTags.Queries.GetTodos;

namespace Todo_App.WebUI.Controllers;
public class TodoItemTagsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<TodoItemTagVm>> Get()
    {
        return await Mediator.Send(new GetTodoItemTagsQuery());
    }

    [HttpGet("GetDashboard")]
    public async Task<IEnumerable<TodoItemTagDashboardDto>> GetDashboard()
    {
        return await Mediator.Send(new GetTodoItemTagsDashboardQuery());
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateTodoItemTagCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateTodoItemTagCommand command)
    {
        if(id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteTodoItemTagCommand(id));
        return NoContent();
    }
}
