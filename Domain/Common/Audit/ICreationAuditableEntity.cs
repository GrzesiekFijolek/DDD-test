namespace Domain.Common.Audit;

public interface ICreationAuditableEntity
{
    CreationAudit CreationInfo { get; set; }
}
