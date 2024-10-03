using TodoApp.Features.Todos;
using TodoApp.Features.Todos.Common;

namespace TodoApp.Integration.Tests.Features.Todos;

public class UpdateTodoCommandTests : IntegrationTestBase
{
    [Fact]
    public async Task UpdateTodo_ReturnsNoContent()
    {
        // Arrange
        var todo = new Todo { Name = "Some Todo", IsComplete = false };
        await using (var db = NewScope().GetDbContext())
        {
            await db.Todos.AddAsync(todo);
            await db.SaveChangesAsync();
        }

        // Act
        var response = await Client.PostAsJsonAsync(
            $"/todos/{todo.Id}",
            new UpdateTodoRequest(todo.Id, "Updated Todo", true),
            JsonSerializerOptions
        );

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var todoResponse = await response.Content.ReadFromJsonAsync<TodoResponse>(
            JsonSerializerOptions
        );

        todoResponse.Should().NotBeNull();
        todoResponse!.Id.Should().Be(todo.Id);
        todoResponse!.Name.Should().Be("Updated Todo");
        todoResponse!.IsComplete.Should().BeTrue();

        await using (var db = NewScope().GetDbContext())
        {
            var updatedTodo = await db.Todos.FindAsync(todo.Id);
            updatedTodo.Should().NotBeNull();
            updatedTodo!.Name.Should().Be("Updated Todo");
            updatedTodo!.IsComplete.Should().BeTrue();
        }
    }
}
