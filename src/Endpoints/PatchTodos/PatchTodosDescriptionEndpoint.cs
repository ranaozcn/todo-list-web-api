using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using todo_list_web_api.Context;
using todo_list_web_api.Models;


public class PatchTodoDescriptionRequest
{
    public string Description { get; set; } = null!;
}

public class PatchTodoDescriptionEndpoint : Endpoint<PatchTodoDescriptionRequest>
{
    private readonly AppDbContext _dbContext;

    public PatchTodoDescriptionEndpoint(AppDbContext db)
    {
        _dbContext = db;
    }

    public override void Configure()
    {
        Patch("/todos/{id:int}/description");
        AllowAnonymous();
    }

    public override async Task HandleAsync(PatchTodoDescriptionRequest req, CancellationToken ct)
    {
        var id = Route<int>("id");
        var todo = await _dbContext.Todos.FindAsync(id);

        if (todo is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        todo.Description = req.Description;
        await _dbContext.SaveChangesAsync(ct);
        await SendOkAsync(ct);
    }
}