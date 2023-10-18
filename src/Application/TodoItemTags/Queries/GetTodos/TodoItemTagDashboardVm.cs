using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo_App.Application.TodoItemTags.Queries.GetTodos;
public class TodoItemTagDashboardVm
{
    public IList<TodoItemTagDto> ItemTags { get; set; } = new List<TodoItemTagDto>();
}
