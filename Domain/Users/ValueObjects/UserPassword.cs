using System.Text.RegularExpressions;
using Domain.Users.Consts;
using Domain.Users.Exceptions;

namespace Domain.Users.ValueObjects;

public sealed record UserPassword
{
    public string Value { get; }

    public UserPassword(string value)
    {
        if (value.Length < UserConsts.UserPassword_MinLength)
            throw new InvalidUserPasswordException($"length must be at least {UserConsts.UserPassword_MinLength}");

        if (value.Length > UserConsts.UserPassword_MaxLength)
            throw new InvalidUserPasswordException($"length must be at most {UserConsts.UserPassword_MaxLength}");

        if (!Regex.IsMatch(value, UserConsts.UserPassword_Regex))
            throw new InvalidUserPasswordException("must contain at least one uppercase letter and one digit, and no whitespace");

        Value = value;
    }

    public static implicit operator UserPassword(string value) => new UserPassword(value);

    public static implicit operator string(UserPassword value) => value.Value;
}
