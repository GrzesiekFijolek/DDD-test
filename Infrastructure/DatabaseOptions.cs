using System.ComponentModel.DataAnnotations;

namespace Infrastructure;

public sealed class DatabaseOptions
{
    public const string SectionName = "database";

    [Required, MinLength(1)]
    public string ConnectionString { get; init; } = null!;

    [Required, MinLength(1)]
    public string MigrationsHistoryTable { get; init; } = "__EFMigrationsHistory";

    [Required, MinLength(1)]
    public string MigrationsHistorySchema { get; init; } = "config";
}