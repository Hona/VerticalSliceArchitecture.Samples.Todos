namespace TodoApp.Domain;

public class Todo
{
    public TodoId Id { get; init; } = TodoId.FromNewGuid();

    public const int MaxNameLength = 50;
    public string Name { get; set; }

    public bool IsComplete { get; set; }
}
