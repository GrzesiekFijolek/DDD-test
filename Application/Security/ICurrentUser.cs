namespace Application.Security;

public interface ICurrentUser
{
    bool IsAuthenticated { get; }

    long? UserId { get; }

    string UserName { get; }
}
