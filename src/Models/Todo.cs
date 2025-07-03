using System;
using System.Collections.Generic;

namespace todo_list_web_api.Models;

public partial class Todo
{
    public int TodoId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool IsCompleted { get; set; }
}
