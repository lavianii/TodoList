using TodoList.Core.Domain;
using TodoList.Core.Microsoft.Extensions.DependencyInjection;
using TodoList.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Globalization;

var config = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.json", true, true)
    .Build();

var services = new ServiceCollection();
var connectionString = config.GetConnectionString("DatabaseConnection");
services.AddCoreDbContext(connectionString);

var serviceProvider = services.BuildServiceProvider();
var dbContext = serviceProvider.GetService<TodoListDbContext>();

async Task AdicionarTarefa()
{
    Console.Write("Digite a descrição da tarefa: ");
    string description = Console.ReadLine() ?? string.Empty;

    if (string.IsNullOrWhiteSpace(description))
    {
        Console.WriteLine("Descrição inválida. Por favor, insira uma descrição válida.");
        await AdicionarTarefa();
        return;
    }

    await dbContext!.Todos!.AddAsync(new TodoItem(description));
    await dbContext!.SaveChangesAsync();
    Console.WriteLine("Tarefa adicionada com sucesso!");
}

async Task RemoverTarefa()
{
    if (!await dbContext!.Todos!.AnyAsync())
    {
        Console.WriteLine("Não há tarefas para remover. A lista de tarefas está vazia.");
        return;
    }

    Console.WriteLine("Tarefas na Lista de Tarefas:");
    await DisplayTasks();

    Console.Write("Digite o ID da tarefa a ser removida: ");

    if (int.TryParse(Console.ReadLine(), out int id) && id >= 0)
    {
        var task = await dbContext!.Todos!.FindAsync(id);

        if (task is null)
        {
            Console.WriteLine("ID inválido, tarefa não encontrada. Por favor, insira um ID válido.");
            return;
        }

        dbContext!.Todos!.Remove(task);
        await dbContext!.SaveChangesAsync();

        Console.WriteLine("Tarefa removida com sucesso!");
        return;
    }

    Console.WriteLine("ID inválido. Por favor, insira um ID válido.");
}

async Task MarcarTarefaParaConcluida()
{
    if (!await dbContext!.Todos!.AnyAsync())
    {
        Console.WriteLine("Não há tarefas para marcar como concluídas. A lista de tarefas está vazia.");
        return;
    }

    Console.WriteLine("Tarefas na Lista de Tarefas:");
    await DisplayTasks();

    Console.Write("Digite o ID da tarefa a ser marcada como concluída: ");

    if (int.TryParse(Console.ReadLine(), out int id) && id >= 0)
    {
        var task = await dbContext!.Todos!.FindAsync(id);

        if (task is null)
        {
            Console.WriteLine("ID inválido, tarefa não encontrada. Por favor, insira um ID válido.");
            return;
        }

        task.IsCompleta = true;
        await dbContext!.SaveChangesAsync();

        Console.WriteLine("Tarefa marcada como concluída!");
        return;
    }

    Console.WriteLine("ID inválido. Por favor, insira um ID válido.");

}

async Task VisualizarTarefas()
{
    if (!await dbContext!.Todos!.AnyAsync())
    {
        Console.WriteLine("Não há tarefas na Lista de Tarefas. Está vazia.");
        return;
    }

    Console.WriteLine("Tarefas na Lista de Tarefas:");
    await DisplayTasks();
}

async Task DisplayTasks()
{
    var todos = await dbContext!.Todos!.ToListAsync();

    foreach (var todo in todos)
        Console.WriteLine($"{todo.Id} - {todo.Descricao} - {(todo.IsCompleta ? "Concluída" : "Não Concluída")}");
}

static void Sair()
{
    Console.WriteLine("Encerrando o Aplicativo de Lista de Tarefas. Adeus!");
    Environment.Exit(0);
}

static void MenuMenu()
{
    Console.WriteLine("\n=====================================");
    Console.WriteLine("Menu:");
    Console.WriteLine("1. Adicionar Tarefa");
    Console.WriteLine("2. Remover Tarefa");
    Console.WriteLine("3. Marcar Tarefa como Concluída");
    Console.WriteLine("4. Visualizar Tarefas");
    Console.WriteLine("5. Sair");
    Console.WriteLine("=====================================\n");

    Console.Write("\nDigite sua escolha (1-5): ");
}

static string VerSO()
{
    var os = Environment.OSVersion.Platform;
    return os switch
    {
        PlatformID.Win32NT => "Windows",
        PlatformID.Unix => "Linux",
        PlatformID.MacOSX => "MacOS",
        _ => "Desconhecido"
    };
}


Console.WriteLine("Bem-vindo ao Aplicativo de Lista de Tarefas!");
Console.WriteLine("Seu sistema operacional é: " + VerSO());

while (true)
{
    MenuMenu();

    int choiceRaw = int.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);

    switch (choiceRaw)
    {
        case 1:
            await AdicionarTarefa();
            break;
        case 2:
            await RemoverTarefa();
            break;
        case 3:
            await MarcarTarefaParaConcluida();
            break;
        case 4:
            await VisualizarTarefas();
            break;
        case 5:
            Sair();
            break;
        default:
            Console.WriteLine("Só pode ser digitado números de 1 a 5.");
            break;
    }
}
