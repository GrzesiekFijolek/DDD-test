namespace Domain.Common.Audit;

public interface ISoftDeleteAuditableEntity
{
    DeletionAudit? DeletionInfo { get; set; }
}
