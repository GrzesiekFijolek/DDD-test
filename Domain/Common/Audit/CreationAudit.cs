namespace Domain.Common.Audit;

public record CreationAudit
{
    public DateTime CreatedAt { get; set; }

    public long? CreatedById { get; set; }

    public string? CreatedByName { get; set; }

    public CreationAudit(DateTime createdAt, long? createdById, string? createdByName)
    {
        CreatedAt = createdAt;
        CreatedById = createdById;
        CreatedByName = createdByName;
    }
}
