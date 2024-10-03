using Microsoft.EntityFrameworkCore;

namespace TodoApp.Common.EfCore;

public class AppDbContext : DbContext
{
    public DbSet<Todo> Todos { get; set; } = default!;

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.RegisterAllInEfCoreConverters(); // Vogen <--> EF Core converters
    }
}
