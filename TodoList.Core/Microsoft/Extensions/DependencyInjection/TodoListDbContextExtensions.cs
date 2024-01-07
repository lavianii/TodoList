using TodoList.Core.Data;
using Microsoft.Extensions.DependencyInjection;

namespace TodoList.Core.Microsoft.Extensions.DependencyInjection;

public static class TodoListDbContextExtensions
{
    public static IServiceCollection AddCoreDbContext(this IServiceCollection services, string connectionString)
        => services.AddSqlite<TodoListDbContext>(connectionString);
}