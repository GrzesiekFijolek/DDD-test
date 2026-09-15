using Domain.Common.Exceptions;

namespace Domain.Users.Exceptions;

internal sealed class InvalidUserNameException : CustomException
{
    public string Reason { get; }

    public InvalidUserNameException(string reason) : base($"Invalid UserName: {reason}")
    {
        Reason = reason;
    }
}
