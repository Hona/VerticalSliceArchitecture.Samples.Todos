using TodoApp.Features.Todos;

namespace TodoApp.Integration.Tests.Features.Todos;

public class CreateTodoCommandTests : IntegrationTestBase
{
    [Fact]
    public async Task CreateTodo_ReturnsTodo()
    {
        // Act
        var response = await Client.PostAsJsonAsync(
            "/todos",
            new CreateTodoRequest("Some Todo"),
            JsonSerializerOptions
        );

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var location = response.Headers.Location;
        location.Should().NotBeNull();
        location!.ToString().Should().StartWith("/todos/");

        var todoIdRaw = location!.ToString().Split('/').Last();
        var todoId = TodoId.Parse(todoIdRaw);
        todoId.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateTodo_WithLongName_ReturnsBadRequest()
    {
        // Act
        var response = await Client.PostAsJsonAsync(
            "/todos",
            new CreateTodoRequest(new string('a', Todo.MaxNameLength + 1)),
            JsonSerializerOptions
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
