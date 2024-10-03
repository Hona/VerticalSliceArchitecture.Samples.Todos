namespace TodoApp.Features.Todos.Common;

public record TodoResponse
{
    public TodoId Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public bool IsComplete { get; init; }
}

[Mapper]
public static partial class TodoResponseMapper
{
    public static partial TodoResponse ToResponse(this Todo source);

    public static partial IQueryable<TodoResponse> ProjectToResponse(this IQueryable<Todo> q);
}
