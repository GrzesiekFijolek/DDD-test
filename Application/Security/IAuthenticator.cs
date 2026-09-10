using Application.Security.Response;

namespace Application.Security;

public interface IAuthenticator
{
    TokenResponse CreateToken(long userId, string role);
}