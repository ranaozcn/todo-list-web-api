using FastEndpoints;
using todo_list_web_api.Context;

public class PatchTodoTitleRequest
{
    public string Title { get; set; } = null!;
}

public class PatchTodoTitleEndpoint : Endpoint<PatchTodoTitleRequest>
{
    private readonly AppDbContext _dbContext;

    public PatchTodoTitleEndpoint(AppDbContext db)
    {
        _dbContext = db;
    }

    public override void Configure()
    {
        Patch("/todos/{id:int}/title");
        AllowAnonymous();
    }

    public override async Task HandleAsync(PatchTodoTitleRequest req, CancellationToken ct)
    {
        var id = Route<int>("id");
        var todo = await _dbContext.Todos.FindAsync(id);

        if (todo is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        todo.Title = req.Title;
        await _dbContext.SaveChangesAsync(ct);
        await SendOkAsync(ct);
    }
}