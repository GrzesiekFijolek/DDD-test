using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Auth;

public sealed class AuthOptions
{
    public const string SectionName = "auth";

    [Required, MinLength(1)]
    public string Issuer { get; init; } = null!;

    [Required, MinLength(1)]
    public string Audience { get; init; } = null!;

    [Required, MinLength(1)]
    public string SigningKey { get; init; } = null!;

    [Required]
    public TimeSpan Expiry { get; init; }
}
