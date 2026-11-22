namespace EWallet.Domain.Entities;

public sealed class AuditLogs
{
    private AuditLogs() {}

    public long Id {get; private set;}

    public long? ActorUserId {get; private set;}
    // public Users? Users {get; private set;} = null!;

    public long? TransactionId {get; private set;} 
    public Transactions? Transactions {get; private set;} = null!;

    public string Action {get; private set;}

    public string EntityType {get; private set;}
    public string? EntityId {get; private set;}

    public string Outcome {get; private set;}

    public string? Description {get; private set;}

    public string? CorrelationId {get; private set;}

    public DateTime CreatedAtUtc {get; private set;}

    public AuditLogs(long? actorUserId, long? transactionId, string action, string entityType, string? entityId, string outcome, string? description, string? correlationId)
    {
        if(actorUserId < 0)
        {
            throw new ArgumentException("Id User Actor tidak boleh negative", nameof(actorUserId));
        }
        if(transactionId < 0)
        {
            throw new ArgumentException("Id Transaction tidak boleh negative", nameof(transactionId));
        }
        if (string.IsNullOrWhiteSpace(action))
        {
            throw new ArgumentException("Action tidak valid", nameof(action));
        }
        if (string.IsNullOrWhiteSpace(entityType))
        {
            throw new ArgumentException("Jenis entity tidak valid", nameof(entityType));
        }
        if (string.IsNullOrWhiteSpace(outcome))
        {
            throw new ArgumentException("Outcome tidak valid", nameof(outcome));
        }

        ActorUserId = actorUserId;
        TransactionId = transactionId;
        Action = action;
        EntityType = entityType;
        EntityId = entityId;
        Outcome = outcome;
        Description = description;
        CorrelationId = correlationId;
        CreatedAtUtc = DateTime.UtcNow;
    }

}