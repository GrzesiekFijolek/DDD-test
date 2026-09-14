namespace Domain.Common.Audit;

public record DeletionAudit
{
    public DateTime DeletedAt { get; set; }

    public long DeletedById { get; set; }

    public string DeletedByName { get; set; }

    public DeletionAudit(DateTime deletedAt, long deletedById, string deletedByName)
    {
        DeletedAt = deletedAt;
        DeletedById = deletedById;
        DeletedByName = deletedByName;
    }
}
