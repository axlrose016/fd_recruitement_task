using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Todo_App.Application.TodoLists.Queries.GetTodos;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.TodoItemTags.Queries.GetTodos;
public class TodoItemTagDashboardDto
{
    public string Title { get; set; } = string.Empty;
    public int Count { get; set; }
}
