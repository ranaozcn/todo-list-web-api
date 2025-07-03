using FastEndpoints;
using todo_list_web_api.Context;

public class PatchTodoIsCompletedRequest
{
    public bool IsCompleted { get; set; }
}

public class PatchTodoIsCompletedEndpoint : Endpoint<PatchTodoIsCompletedRequest>
{
    private readonly AppDbContext _dbContext;

    public PatchTodoIsCompletedEndpoint(AppDbContext db)
    {
        _dbContext = db;
    }   
        
    public override void Configure()
    {
        Patch("/todos/{id:int}/iscompleted");
        AllowAnonymous();
    }

    public override async Task HandleAsync(PatchTodoIsCompletedRequest req, CancellationToken ct)
    {   
        var id = Route<int>("id");
        var todo = await _dbContext.Todos.FindAsync(id);

        if (todo is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        todo.IsCompleted = req.IsCompleted;
        await _dbContext.SaveChangesAsync(ct);
        await SendOkAsync(ct);
    }
}