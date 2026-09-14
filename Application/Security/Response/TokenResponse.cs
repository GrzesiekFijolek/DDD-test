namespace Application.Security.Response;

public record TokenResponse
{
    public required string AccessToken { get; set; }
}
