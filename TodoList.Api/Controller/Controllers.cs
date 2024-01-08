using Microsoft.EntityFrameworkCore;
using TodoList.Core.Data;
using TodoList.Core.Domain;

namespace TodoList.Api.Controllers
{

    public static class Controllers
    {

        public static async Task<IResult> GetAllTodosAsync(TodoListDbContext data)
        {
            var todos = await data.Todos!.ToListAsync();
            return Results.Ok(todos);
        }

        public static async Task<IResult> GetTodosAsync(TodoListDbContext data, int id)
        {
            var todos = await data.Todos!.FindAsync(id);

            if (todos is null) return Results.NotFound();

            return Results.Ok(todos);
        }

        public static async Task<IResult> PostTodoAsync(TodoListDbContext data, TodoItem todoItem)
        {
            await data.Todos!.AddAsync(todoItem);
            await data.SaveChangesAsync();

            return Results.Created($"/todos/{todoItem.Id}", todoItem);
        }

        public static async Task<IResult> UpdateTodoAsync(TodoListDbContext data, TodoItem updatedTodo, int id)
        {

            var todo = await data.Todos!.FindAsync(id);
            if (todo is null)
                return Results.NotFound();

            todo.Descricao = updatedTodo.Descricao;

            await data.SaveChangesAsync();
            return Results.Ok();

        }

        public static async Task<IResult> DeleteTodoAsync(TodoListDbContext data, int id)
        {
            var todo = await data.Todos!.FindAsync(id);
            if (todo is null) return Results.NotFound();

            data.Todos.Remove(todo);
            await data.SaveChangesAsync();

            return Results.Ok();
        }
    }
}