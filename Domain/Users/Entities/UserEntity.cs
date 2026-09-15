using Domain.Common.Audit;
using Domain.Common.ValueObjects;
using Domain.Users.ValueObjects;

namespace Domain.Users.Entities;

public abstract class UserEntity : IModificationAuditableEntity, ISoftDeleteAuditableEntity
{
    public long Id { get; set; }

    public Email Email { get; set; } = default!;

    public UserName UserName { get; set; } = default!;

    public UserPassword Password { get; set; } = default!;

    public ModificationAudit? ModificationInfo { get; set; }

    public DeletionAudit? DeletionInfo { get; set; }

    protected UserEntity()
    {
    }

    protected UserEntity(string email, string userName, string password)
    {
        Email = email;
        UserName = userName;
        Password = password;
    }
}
