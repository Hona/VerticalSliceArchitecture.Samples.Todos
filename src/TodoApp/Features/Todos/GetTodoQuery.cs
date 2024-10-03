using Microsoft.EntityFrameworkCore;
using TodoApp.Features.Todos.Common;

namespace TodoApp.Features.Todos;

public sealed record GetTodoRequest(TodoId TodoId);

internal sealed class GetTodoQuery(AppDbContext db)
    : Endpoint<GetTodoRequest, Results<Ok<TodoResponse>, NotFound>>
{
    public override void Configure()
    {
        Get("/todos/{TodoId}");
        Summary(x =>
        {
            x.Description = "Views the todo";
        });
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        GetTodoRequest request,
        CancellationToken cancellationToken
    )
    {
        var response = await db
            .Todos.AsNoTracking()
            .Where(x => x.Id == request.TodoId)
            .ProjectToResponse()
            .FirstOrDefaultAsync(cancellationToken);

        if (response is null)
        {
            await SendResultAsync(TypedResults.NotFound());
            return;
        }

        await SendResultAsync(TypedResults.Ok(response));
    }
}
