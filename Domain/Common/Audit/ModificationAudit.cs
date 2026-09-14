namespace Domain.Common.Audit;

public record ModificationAudit
{
    public DateTime ModifiedAt { get; set; }

    public long ModifiedById { get; set; }

    public string ModifiedByName { get; set; }

    public ModificationAudit(DateTime modifiedAt, long modifiedById, string modifiedByName)
    {
        ModifiedAt = modifiedAt;
        ModifiedById = modifiedById;
        ModifiedByName = modifiedByName;
    }
}
