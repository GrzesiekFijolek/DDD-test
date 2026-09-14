using Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure.Database;

internal sealed class AppDbContext : DbContext
{
    private readonly string _defaultSchema;

    public DbSet<UserEntity> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options, IOptions<DatabaseOptions> databaseOptions) : base(options)
    {
        _defaultSchema = databaseOptions.Value.DefaultSchema;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(_defaultSchema);

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
