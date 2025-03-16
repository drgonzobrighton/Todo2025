using System.Globalization;

namespace TodoApp.Presentation;

public static class ConsoleHelper
{
    public static string GetSelection(string prompt, List<string> validOptions)
    {
        var optionValues = validOptions.Select(x => $"'{x}'").ToArray();
        return GetInput<string>(prompt, Validate)!;

        Result Validate(string? value) =>
            validOptions.Contains(value ?? string.Empty, StringComparer.OrdinalIgnoreCase)
                ? Result.Ok()
                : new Error($"'{value ?? string.Empty}' is not a valid option. Please choose from {string.Join(" or ", optionValues)}");
    }

    public static T? GetInput<T>(string prompt, Func<T?, Result>? validateFunc = null)
    {
        T? result = default;

        Print(prompt);

        while (true)
        {
            var userInput = Console.ReadLine();

            var isOptional = Nullable.GetUnderlyingType(typeof(T)) is not null;

            if (string.IsNullOrEmpty(userInput) && isOptional)
                return default;

            try
            {
                var type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
                result = (T)Convert.ChangeType(userInput, type, CultureInfo.InvariantCulture)!;
            }
            catch (Exception)
            {
                PrintError($"Input is not valid, expected a {GetTypeName(result)}");
                continue;
            }

            if (validateFunc is null)
            {
                return result;
            }

            var validationResult = validateFunc(result);

            if (validationResult.Success)
            {
                return result;
            }

            PrintError(validationResult.Error.Message);
        }
    }

    public static void PrintError(string message) => Print(message, ConsoleColor.Red);

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

    private static string GetTypeName<T>(T o)
    {
        return o switch
        {
            int or double or long or float => "number",
            DateTime => "date",
            _ => o.GetType().Name
        };
    }
}
