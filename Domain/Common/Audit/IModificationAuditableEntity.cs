namespace Domain.Common.Audit;

public interface IModificationAuditableEntity
{
    ModificationAudit? ModificationInfo { get; set; }
}
