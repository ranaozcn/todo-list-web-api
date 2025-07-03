using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using todo_list_web_api.Context;
using todo_list_web_api.Models;

namespace todo_list_web_api.Endpoints.GetTodos
{

    public class GetTodosResponse
    {
        public List<Todo> Todos { get; set; }
    }

    public class GetTodosEndpoint : EndpointWithoutRequest<GetTodosResponse>
    {
        private readonly AppDbContext _dbContext;

        public GetTodosEndpoint(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // Endpoint'in hangi adresi dinleyceğini burda karar veriyoruz
        public override void Configure()
        {
            Get("/todos");
            AllowAnonymous();
        }
        // Bir istek geldiğinde çalışır
        public override async Task HandleAsync(CancellationToken ct)
        {
            // Veri tabanından Todos tablosundaki görevleri asenkron bir şekilde alıyoruz
            var todos = await _dbContext.Todos.ToListAsync(ct);

            // Aldığımız görevleri kullanıcıya gönderiyoruz
            await SendAsync(new() { Todos = todos });
        }
    }
}