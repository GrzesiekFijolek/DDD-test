namespace Application.Security.Requests;

public record TokenRequest
{
    public string AccessToken { get; set; }
}