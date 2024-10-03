using Microsoft.Extensions.DependencyInjection;
using TodoApp.Common.EfCore;

namespace TodoApp.Integration.Tests;

public static class ScopeExtensions
{
    public static AppDbContext GetDbContext(this AsyncServiceScope scope) =>
        scope.ServiceProvider.GetRequiredService<AppDbContext>();
}
