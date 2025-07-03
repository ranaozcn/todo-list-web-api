using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using todo_list_web_api.Context;
using todo_list_web_api.Models;

namespace todo_list_web_api.Endpoints.PostTodos
{
    public class CreateTodoRequest
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsCompleted { get; set; }
    }

    public class CreateTodoResponse
    {
        public int TodoId { get; set; }
    }
    public class PostTodosEndpoint : Endpoint <CreateTodoRequest,CreateTodoResponse>
    {
        private readonly AppDbContext _dbContext;
        public PostTodosEndpoint (AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override void Configure()
        {
            Post("/todos");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CreateTodoRequest request, CancellationToken ct)
        {
            var todo = new Todo();
            todo.Title = request.Title;
            todo.Description = request.Description;
            todo.IsCompleted = request.IsCompleted;
            
            // Görev veri tabanına eklenir sonra kaydediyoruz
            _dbContext.Todos.Add(todo);
            await _dbContext.SaveChangesAsync(ct);

            // Kullanıcıya da eklenen görevin id bilgisini gönderiyoruz
            await SendAsync(new CreateTodoResponse { TodoId = todo.TodoId });   

        }
    }
}
