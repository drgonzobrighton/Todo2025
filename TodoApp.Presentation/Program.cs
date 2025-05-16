using ConsoleTables;
using TodoApp.Application.CreateTodoItem;
using TodoApp.Application.Models;
using TodoApp.Application.Persistence;
using TodoApp.Presentation;

var repo = new InMemoryTodoRepository();
var menu = new ConsoleMenu();
menu.AddItem("List all items", ListAllItems);
menu.AddItem("Create item", CreateItem);
menu.AddItem("Get item", GetById);
menu.AddItem("Update item", () => ConsoleHelper.Print("Updating item"), 'u');
menu.AddItem("Delete item", () => ConsoleHelper.Print("Deleting item"), 'd');
menu.AddItem("Exit", () => Environment.Exit(0), 'x');

menu.Show();

void CreateItem()
{
    var title = ConsoleHelper.GetInput<string>("Enter title: ", ValidateNonNullString)!;
    var description = ConsoleHelper.GetInput<string?>("Enter description: ", isOptional: true);
    var completeBy = ConsoleHelper.GetInput<DateTime?>("Enter complete by: ");

    var command = new CreateTodoItemCommand(title, description, completeBy);
    var handler = new CreateTodoItemCommandHandler(repo);
    var id = handler.Handle(command);

    ConsoleHelper.Print($"The item has been created: {id}");
}

void ListAllItems()
{
    var items = repo.GetAll();

    ConsoleTable.From(items).Write();
}

void GetById()
{
    var id = ConsoleHelper.GetInput<string>("Enter Id: ")!;

    if (!Guid.TryParse(id.Trim(), out var guidId))
    {
        ConsoleHelper.PrintError("Invalid id format");
        return;
    }
    var todoItem = repo.GetById(guidId);

    if (todoItem is null)
    {
        ConsoleHelper.PrintError("Item does not exist");
        return;
    }

    IEnumerable<TodoItem> items = [todoItem];
    
    ConsoleTable.From(items).Write();
}

static Result ValidateNonNullString(string? value) => string.IsNullOrEmpty(value)
    ? Result.Fail(new Error("Non empty value is expected."))
    : Result.Ok();
