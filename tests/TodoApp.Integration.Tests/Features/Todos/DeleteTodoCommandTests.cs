namespace TodoApp.Integration.Tests.Features.Todos;

public class DeleteTodoCommandTests : IntegrationTestBase
{
    [Fact]
    public async Task DeleteTodo_ReturnsNoContent()
    {
        // Arrange
        var todo = new Todo { Id = TodoId.FromNewGuid(), Name = "Some Todo" };

        await using (var db = NewScope().GetDbContext())
        {
            db.Add(todo);
            await db.SaveChangesAsync();
        }

        // Act
        var response = await Client.DeleteAsync($"/todos/{todo.Id}");

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        await using (var db = NewScope().GetDbContext())
        {
            var foundTodo = await db.FindAsync<Todo>(todo.Id);
            foundTodo.Should().BeNull();
        }
    }
}
