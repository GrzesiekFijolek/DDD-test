using Domain.Common.Exceptions;

namespace Infrastructure.Database.Exceptions;

internal sealed class UnauthenticatedAuditActionException : CustomException
{
    public string Action { get; }

    public UnauthenticatedAuditActionException(string action)
        : base($"Cannot {action} an auditable entity without an authenticated user with a resolvable id and name.")
    {
        Action = action;
    }
}
