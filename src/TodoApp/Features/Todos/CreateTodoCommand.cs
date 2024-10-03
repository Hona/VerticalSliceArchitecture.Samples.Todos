namespace TodoApp.Features.Todos;

public sealed record CreateTodoRequest(string Name);

internal sealed class CreateTodoRequestValidator : AbstractValidator<CreateTodoRequest>
{
    public CreateTodoRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(Todo.MaxNameLength);
    }
}

internal sealed class CreateTodoCommand(AppDbContext db)
    : Endpoint<CreateTodoRequest, Results<Created, BadRequest>>
{
    public override void Configure()
    {
        Post("/todos");
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        CreateTodoRequest request,
        CancellationToken cancellationToken
    )
    {
        var todo = new Todo { Id = TodoId.FromNewGuid(), Name = request.Name };
        db.Add(todo);
        await db.SaveChangesAsync(cancellationToken);

        await SendResultAsync(TypedResults.Created($"/todos/{todo.Id}"));
    }
}
