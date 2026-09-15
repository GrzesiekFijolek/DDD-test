using Domain.Users.Consts;
using Domain.Users.Exceptions;

namespace Domain.Users.ValueObjects;

public sealed record UserName
{
    public string Value { get; }

    public UserName(string value)
    {
        if (value.Length < UserConsts.UserName_MinLength)
            throw new InvalidUserNameException($"length must be at least {UserConsts.UserName_MinLength}");

        if (value.Length > UserConsts.UserName_MaxLength)
            throw new InvalidUserNameException($"length must be at most {UserConsts.UserName_MaxLength}");

        Value = value;
    }

    public static implicit operator UserName(string value) => new UserName(value);

    public static implicit operator string(UserName value) => value.Value;
}
