using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Security;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Auth;

internal sealed class HttpContextCurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextCurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    public long? UserId
    {
        get
        {
            var raw = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                      ?? _httpContextAccessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            return long.TryParse(raw, out var id) ? id : null;
        }
    }

    public string UserName =>
        _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value
        ?? _httpContextAccessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.UniqueName)?.Value;
}
