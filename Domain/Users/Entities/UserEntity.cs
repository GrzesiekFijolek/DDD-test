using Domain.Common.Audit;

namespace Domain.Users.Entities;

public abstract class UserEntity : IModificationAuditableEntity, ISoftDeleteAuditableEntity
{
    public long Id { get; set; }

    public required string Email { get; set; }

    public required string UserName { get; set; }

    public ModificationAudit? ModificationInfo { get; set; }

    public DeletionAudit? DeletionInfo { get; set; }
}
