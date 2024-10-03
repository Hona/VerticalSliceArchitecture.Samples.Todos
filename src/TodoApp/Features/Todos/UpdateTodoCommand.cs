using TodoApp.Features.Todos.Common;

namespace TodoApp.Features.Todos;

public sealed record UpdateTodoRequest(TodoId TodoId, string? Name, bool? IsComplete);

internal sealed class UpdateTodoCommand(AppDbContext db)
    : Endpoint<UpdateTodoRequest, Results<Ok<TodoResponse>, NotFound>>
{
    public override void Configure()
    {
        Post("/todos/{TodoId}");
        Summary(x =>
        {
            x.Description = "Update the text or completion status of a Todo";
        });
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        UpdateTodoRequest request,
        CancellationToken cancellationToken
    )
    {
        var todo = await db.FindAsync<Todo>(request.TodoId, cancellationToken);

        if (todo is null)
        {
            await SendResultAsync(TypedResults.NotFound());
            return;
        }

        if (request.Name is not null)
        {
            todo.Name = request.Name;
        }

        if (request.IsComplete is not null)
        {
            todo.IsComplete = request.IsComplete.Value;
        }

        await db.SaveChangesAsync(cancellationToken);

        var output = todo.ToResponse();
        await SendResultAsync(TypedResults.Ok(output));
    }
}
