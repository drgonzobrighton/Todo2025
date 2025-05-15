using TodoApp.Application.Models;

namespace TodoApp.Application.Persistence;

public class InMemoryTodoRepository : ITodoRepository
{
    private readonly Dictionary<Guid, TodoItem> _items = [];

    public void Save(TodoItem item)
    {
        _items[item.Id] = item;
    }

    public TodoItem? GetById(Guid id) => _items.GetValueOrDefault(id);

    public List<TodoItem> GetAll() => _items.Values.ToList();

    public void Delete(Guid id)
    {
        _items.Remove(id);
    }
}
