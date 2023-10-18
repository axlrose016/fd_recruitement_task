using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo_App.Application.TodoItemTags.Queries.GetTodos;
public class TodoItemTagDashboardDto
{
    public string Title { get; set; } = string.Empty;
    public int Count { get; set; }
}
