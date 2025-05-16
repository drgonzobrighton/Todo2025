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
                : new Error($"'{value ?? string.Empty}' is not a valid option. Please choose from {string.Join(", ", optionValues)}");
    }

    public static T? GetInput<T>(string prompt, Func<T?, Result>? validateFunc = null, bool? isOptional = null)
    {
        T? result = default;

        while (true)
        {
            Print(prompt);

            var userInput = Console.ReadLine();
            var underlyingType = Nullable.GetUnderlyingType(typeof(T));
            var targetType = underlyingType ?? typeof(T);
            isOptional ??= underlyingType is not null;

            if (string.IsNullOrEmpty(userInput) && isOptional.Value)
                return result;

            try
            {
                result = (T)Convert.ChangeType(userInput, targetType, CultureInfo.InvariantCulture)!;
            }
            catch (Exception)
            {
                PrintError($"Invalid value, expected a {GetTypeName(result)}");
                continue;
            }

            if (validateFunc is null)
                return result;

            var validationResult = validateFunc(result);

            if (validationResult.Success)
                return result;

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

    private static string GetTypeName<T>(T o) =>
        o switch
        {
            int or double or long or float => "number",
            DateTime => "date",
            _ => o?.GetType().Name ?? string.Empty
        };
}
