namespace TodoApp;

public static class ConsoleHelper
{
    public static string GetSelection(string prompt, List<string> validOptions, string? validationMessage = null)
    {
        Print(prompt);

        var optionValues = validOptions.Select(x => $"'{x}'").ToArray();

        var input = Console.ReadLine();

        while (string.IsNullOrWhiteSpace(input) || !validOptions.Contains(input, StringComparer.CurrentCultureIgnoreCase))
        {
            validationMessage ??= $"'{input ?? string.Empty}' is not a valid option. Please choose from {string.Join(" or ", optionValues)}";
            Print(validationMessage, ConsoleColor.Red);
            input = Console.ReadLine();
        }

        var action = () => Console.Write(optionValues[0]);


        return input;
    }

    public static void Print(string message, ConsoleColor? color = null)
    {
        if (color is not null)
        {
            Console.ForegroundColor = color.Value;
        }

        Console.WriteLine();
        Console.WriteLine(message);
        Console.ResetColor();
    }
}
