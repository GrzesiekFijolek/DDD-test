using Domain.Common.Exceptions;
using Domain.Users.ValueObjects;

namespace Domain.Users.Exceptions;

internal sealed class InvalidUserPasswordException : CustomException
{
    public string Reason { get; }

    public InvalidUserPasswordException(string reason) : base($"Invalid Password: {reason}")
    {
        Reason = reason;
    }
}
