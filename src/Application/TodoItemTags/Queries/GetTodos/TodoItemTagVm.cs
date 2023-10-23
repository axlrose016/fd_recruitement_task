using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo_App.Application.TodoItemTags.Queries.GetTodos;

public class TodoItemTagVm
{
    public IList<TodoItemTagDto> ItemTags { get; set; } = new List<TodoItemTagDto>();
}
