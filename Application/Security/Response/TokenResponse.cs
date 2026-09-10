namespace Application.Security.Response;

public record TokenResponse
{
    public string AccessToken { get; set; }
}