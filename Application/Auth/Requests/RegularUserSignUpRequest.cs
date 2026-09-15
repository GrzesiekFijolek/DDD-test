namespace Application.Auth.Requests;

public record RegularUserSignUpRequest(string Email, string UserName, string Password, string Department);