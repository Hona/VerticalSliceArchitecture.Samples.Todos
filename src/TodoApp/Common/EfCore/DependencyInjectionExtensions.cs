using Microsoft.EntityFrameworkCore;

namespace TodoApp.Common.EfCore;

public static class DependencyInjectionExtensions
{
    public static void AddAppDbContext(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });
    }
}
