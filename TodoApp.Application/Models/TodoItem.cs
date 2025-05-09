namespace TodoApp.Application.Models;

public record TodoItem
{
    public Guid Id { get; init; }
    public required string Title { get; set; }
    public bool IsCompleted { get; set; }
    public string? Description { get; set; }
    public DateTime? CompleteBy { get; set; }
}
