namespace TodoApp;

public class ConsoleMenu(List<ConsoleMenuItem> menuItems)
{
    private readonly Dictionary<string, ConsoleMenuItem> _menuItems = menuItems.ToDictionary(x => x.Key);

    public void Show()
    {
        Console.WriteLine();

        foreach (var (_, menuItem) in _menuItems)
        {
            Console.WriteLine($"{menuItem.Label} - {menuItem.Key}");
            Console.WriteLine();
        }

        Console.WriteLine();

        var option = ConsoleHelper.GetSelection(
            "Please select an option from above or press [x] to exit",
            [.._menuItems.Select(x => x.Key), "x"]);

        if (option == "x")
        {
            return;
        }

        _menuItems[option].ShowScreen.Invoke();
    }
}

public class ConsoleMenuItem(string label, Action showScreen, string? key = null)
{
    public string Label { get; } = label;
    public Action ShowScreen { get; } = showScreen;
    public string Key { get; } = string.IsNullOrEmpty(key)
        ? label.First().ToString().ToLower()
        : key.ToLower();
}
