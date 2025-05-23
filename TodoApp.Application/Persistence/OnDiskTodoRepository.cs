using System.Text.Json;
using TodoApp.Application.Models;

namespace TodoApp.Application.Persistence;

public class OnDiskTodoRepository : ITodoRepository
{
    private readonly string _filePath;
    private readonly Dictionary<Guid, TodoItem> _items = new();

    public OnDiskTodoRepository()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var appFolder = Path.Combine(appData, "TodoApp");
        Directory.CreateDirectory(appFolder);

        _filePath = Path.Combine(appFolder, "todos.json");

        if (File.Exists(_filePath))
        {
            var json = File.ReadAllText(_filePath);
            var items = JsonSerializer.Deserialize<List<TodoItem>>(json) ?? [];
            _items = items.ToDictionary(x => x.Id);
        }
    }

    public void Save(TodoItem item)
    {
        _items[item.Id] = item;
        SaveToDisk();
    }

    public TodoItem? GetById(Guid id) => _items.GetValueOrDefault(id);

    public List<TodoItem> GetAll() => _items.Values.ToList();

    public void Delete(Guid id)
    {
        _items.Remove(id);
        SaveToDisk();
    }

    private void SaveToDisk()
    {
        var json = JsonSerializer.Serialize(_items.Values.ToList(), new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}
