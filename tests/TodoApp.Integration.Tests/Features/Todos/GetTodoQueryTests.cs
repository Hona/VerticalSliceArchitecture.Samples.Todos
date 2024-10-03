using TodoApp.Features.Todos;
using TodoApp.Features.Todos.Common;

namespace TodoApp.Integration.Tests.Features.Todos;

public class GetTodoQueryTests : IntegrationTestBase
{
    [Fact]
    public async Task GetTodo_ReturnsTodo()
    {
        var todo = new Todo { Name = "Some Todo", IsComplete = false };

        // Arrange
        await using (var scope = NewScope())
        {
            var db = scope.GetDbContext();

            await db.Todos.AddAsync(todo);
            await db.SaveChangesAsync();
        }

        // Act
        var response = await Client.GetAsync($"/todos/{todo.Id}");

        // Assert
        response.EnsureSuccessStatusCode();
        var todoResponse = await response.Content.ReadFromJsonAsync<TodoResponse>();
        todoResponse
            .Should()
            .BeEquivalentTo(
                new TodoResponse
                {
                    Id = todo.Id,
                    Name = todo.Name,
                    IsComplete = todo.IsComplete,
                }
            );
    }

    [Fact]
    public async Task GetTodo_WhenNotFound_ReturnsNotFound()
    {
        // Act
        var response = await Client.GetAsync($"/todos/{TodoId.FromNewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetTodo_AfterCreation_ReturnsSameTodo()
    {
        // Arrange
        var todo = new CreateTodoRequest("Some Todo");
        var response = await Client.PostAsJsonAsync("/todos", todo);
        response.EnsureSuccessStatusCode();
        var location = response.Headers.Location;

        // Act
        response = await Client.GetAsync(location);

        // Assert
        response.EnsureSuccessStatusCode();
        var todoResponse = await response.Content.ReadFromJsonAsync<TodoResponse>();
        todoResponse
            .Should()
            .BeEquivalentTo(
                new TodoResponse
                {
                    Id = todoResponse.Id,
                    Name = todo.Name,
                    IsComplete = false,
                }
            );
    }
}
