using System.ComponentModel.DataAnnotations;
using TodoApp.Application.Models;

namespace TodoApp.Application.CreateTodoItem;

public record CreateTodoItemCommand(string Title, string? Description, DateTime? CompleteBy);

public class CreateTodoItemCommandHandler
{
    public Guid Handle(CreateTodoItemCommand command)
    {
        Validate(command);

        var todoItem = new TodoItem
        {
            Id = Guid.NewGuid(),
            Title = command.Title,
            Description = command.Description,
            CompleteBy = command.CompleteBy
        };

        return todoItem.Id;
    }

    private static void Validate(CreateTodoItemCommand command)
    {
        var (title, description, _) = command;

        if (string.IsNullOrEmpty(title))
        {
            throw new ValidationException("Title is required");
        }

        if (description is not null && description.Length <= 0)
        {
            throw new ValidationException("Description cannot be empty");
        }
    }
}
