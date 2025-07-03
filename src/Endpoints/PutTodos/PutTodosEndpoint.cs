using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using todo_list_web_api.Context;
using todo_list_web_api.Models;

namespace todo_list_web_api.Endpoints.PutTodos
{
    public class UpdateTodoRequest
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsCompleted { get; set; }
    }

    public class UpdateTodoResponse 
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;

    }
    public class PutTodosEndpoint : Endpoint<UpdateTodoRequest, UpdateTodoResponse>
    {
        private readonly AppDbContext _dbContext;
        public PutTodosEndpoint(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override void Configure()
        {
            Put("/todos/{id:int}");
            AllowAnonymous();
        }
        public override async Task HandleAsync(UpdateTodoRequest request, CancellationToken ct)
        {
            // Request Url'inde id bilgisini alır sonrasında bunu veri tabanında arar
            var id = Route<int>("id");
            var todo = await _dbContext.Todos.FirstOrDefaultAsync(t => t.TodoId == id);
            if(todo is null)
            {  
                // Eğer boşsa kullanıcıya bulunamadığını ve hata kodunu gönderir
                await SendAsync(new UpdateTodoResponse
                {
                    Success = false,
                    Message = $"Görev bulunamadı: id={id}"
                }, statusCode: 404);
                return;
            }
            // Eğer görev bulunduysa günceller veri tabanına güncel halini kayıt eder ve kullanıcıya işlemin başarılı mesajını gönderir  
            todo.Title = request.Title;
            todo.Description = request.Description;
            todo.IsCompleted = request.IsCompleted;
            await _dbContext.SaveChangesAsync(ct);

            await SendAsync(new UpdateTodoResponse
            {
                Success = true,
                Message = $"Görev güncellendi: id={id}"
            });

        }
    }
}
