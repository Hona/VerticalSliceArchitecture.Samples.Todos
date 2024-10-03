using Microsoft.EntityFrameworkCore;

namespace TodoApp.Features.Todos;

public sealed record DeleteTodoRequest(TodoId TodoId);

internal sealed class DeleteTodoCommand(AppDbContext db)
    : Endpoint<DeleteTodoRequest, Results<NoContent, NotFound>>
{
    public override void Configure()
    {
        Delete("/todos/{TodoId}");
        Summary(x =>
        {
            x.Description = "Remove a todo";
        });
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        DeleteTodoRequest request,
        CancellationToken cancellationToken
    )
    {
        var rowsEffected = await db
            .Todos.Where(x => x.Id == request.TodoId)
            .ExecuteDeleteAsync(cancellationToken);

        if (rowsEffected == 0)
        {
            await SendResultAsync(TypedResults.NotFound());
            return;
        }

        await SendResultAsync(TypedResults.NoContent());
    }
}
