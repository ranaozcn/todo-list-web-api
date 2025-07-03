using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using todo_list_web_api.Context;
using todo_list_web_api.Models;

namespace todo_list_web_api.Endpoints.DeleteTodos;

public class DeleteTodosByIdEndpoint : EndpointWithoutRequest
{
    private readonly AppDbContext _dbContext;

    public DeleteTodosByIdEndpoint(AppDbContext db)
    {
        _dbContext = db;
    }

    public override void Configure()
    {
        Delete("/todos/{id:int}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("id");

        var todo = await _dbContext.Todos.FirstOrDefaultAsync(t => t.TodoId == id, ct);

        if (todo is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        _dbContext.Todos.Remove(todo);
        await _dbContext.SaveChangesAsync(ct);

        await SendOkAsync(ct);
    }
}