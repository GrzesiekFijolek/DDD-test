namespace Application.Security.Requests;

public record TokenRequest
{
    public required string AccessToken { get; set; }
}
