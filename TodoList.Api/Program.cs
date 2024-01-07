using TodoList.Core.Microsoft.Extensions.DependencyInjection;
using TodoList.Api.Controllers;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");
builder.Services.AddCoreDbContext(connectionString);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "TodoList API",
        Description = "Armazena uma lista de tarefas",
        Version = "v1"
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => 
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoList API");
});

app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

app.MapGet("/todos", Controllers.GetAllTodosAsync);
app.MapGet("todos/{id}", Controllers.GetTodosAsync);
app.MapPost("/todos", Controllers.PostTodoAsync);
app.MapDelete("/todos/{id}", Controllers.DeleteTodoAsync);


app.Run();
