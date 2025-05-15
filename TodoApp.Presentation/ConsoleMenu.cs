namespace TodoApp.Presentation;

public class ConsoleMenu
{
    private readonly List<MenuItem> _menuItems = [];

    private sealed record MenuItem(string Description, Action Action, char Key);

    public void AddItem(string description, Action action, char? key = null)
    {
        key ??= char.ToLower(description[0]);

        if (_menuItems.Any(menuItem => menuItem.Key == key))
        {
            throw new ArgumentException($"Key {key} already exists");
        }

        _menuItems.Add(new MenuItem(description, action, key.Value));
    }

    public void Show()
    {
        var choices = _menuItems.Select(m => m.Key.ToString()).ToList();

        while (true)
        {
            ConsoleHelper.Print("Select an option:");
            foreach (var item in _menuItems)
            {
                ConsoleHelper.Print($"[{item.Key}] {item.Description}", ConsoleColor.Gray);
            }

            var choice = ConsoleHelper.GetSelection("Enter your choice: ", choices);
            var menuItem = _menuItems.Find(item => item.Key.ToString() == choice)!;

            menuItem.Action();

            choice = ConsoleHelper.GetSelection("Would you like to select another item? (y/n):", ["y", "n"]);

            if (choice.ToLower() == "n")
                return;
        }
    }
}
