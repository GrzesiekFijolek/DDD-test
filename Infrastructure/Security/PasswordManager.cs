using Application.Security;
using Domain.Users.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Security;

internal sealed class PasswordManager : IPasswordManager
{
    private readonly IPasswordHasher<UserEntity> _passwordHasher;

    public PasswordManager(IPasswordHasher<UserEntity> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public string Secure(string password)
    {
        return _passwordHasher.HashPassword(default!, password);
    }

    public bool Validate(string password, string securedPassword)
    {
        return _passwordHasher.VerifyHashedPassword(default!, securedPassword, password) == PasswordVerificationResult.Success;
    }
}