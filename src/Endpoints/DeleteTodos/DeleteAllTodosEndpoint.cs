using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using todo_list_web_api.Context;
using todo_list_web_api.Endpoints.PostTodos;
using todo_list_web_api.Models;

namespace todo_list_web_api.Endpoints.DeleteTodos
{
    public class DeleteAllTodosEndpoint : EndpointWithoutRequest
    {
        private readonly AppDbContext _dbContext;
        public DeleteAllTodosEndpoint(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public override void Configure()
        {
            Delete("/todos");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var allTodos = await _dbContext.Todos.ToListAsync(ct);
            //Hiç görev yoksa 204 yanıtı gönderilir
            if (allTodos.Count == 0)
            {
                await SendNoContentAsync(ct);
                return;
            }
            //Görevlerin hepisni tek seferde siler yapılan değişiklik veri tabanına kayıt edilir
            _dbContext.Todos.RemoveRange(allTodos);
            await _dbContext.SaveChangesAsync(ct);
            // İşlem başarılı olunca 200 yanıtı gönderilir 
            await SendOkAsync(ct);

        }

    }
}
