using Microsoft.EntityFrameworkCore;
using TodoList.Core.Domain;

namespace TodoList.Core.Data
{
    public class TodoListDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<TodoItem>? Todos { get; set; }
    }
}