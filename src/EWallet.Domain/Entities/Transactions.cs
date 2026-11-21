namespace EWallet.Domain.Entities;

public sealed class Transactions
{
    private Transactions() {}
    public long Id {get; private set;}
    public string ReferenceNo {get; private set;}

    public long? SenderWalletId {get; private set;}
    public Wallets? WalletsSender {get; private set;}

    public long ReceiverWalletId {get; private set;}
    public Wallets WalletsReceiver {get; private set;} = null!;

    public string Type {get; private set;}
    public decimal Amount {get; private set;}
    public string Status {get; private set;}
    public string? Description {get; private set;}
    public DateTime CreatedAtUtc {get; private set;}
    public DateTime? CompletedAtUtc {get; private set;}

    public ICollection<WalletMutations> WalletMutations {get; private set;} = [];
    public ICollection<AuditLogs> AuditLogs {get; private set;} = [];
    public TopUpRequests? TopUpRequests {get; private set;}


    public Transactions(string referenceNo, long? senderWalletId, long receiverWalletId, string type, decimal amount, string? description)
    {
        if (string.IsNullOrWhiteSpace(referenceNo))
        {
            throw new ArgumentException("Referensi tidak valid", nameof(referenceNo));
        }
        if(senderWalletId < 0)
        {
            throw new ArgumentException("SenderId tidak valid, must be positive id", nameof(senderWalletId));
        }
        if(receiverWalletId < 0)
        {
            throw new ArgumentException("ReceiverId tidak valid, must be positive id", nameof(receiverWalletId));
        }
        if (string.IsNullOrWhiteSpace(type))
        {
            throw new ArgumentException("type is required", nameof(type));
        }
        if(amount <= 0)
        {
            throw new ArgumentException("Amount tidak valid, must be positive value", nameof(amount));
        }

        ReferenceNo = referenceNo;
        SenderWalletId = senderWalletId;
        ReceiverWalletId = receiverWalletId;
        Type = type;
        Amount = amount;
        Status = "PENDING";
        Description = description;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public Transactions(string referenceNo, long receiverWalletId, string type, decimal amount, string? description)
    {
        if (string.IsNullOrWhiteSpace(referenceNo))
        {
            throw new ArgumentException("Referensi tidak valid", nameof(referenceNo));
        }
        if(receiverWalletId < 0)
        {
            throw new ArgumentException("ReceiverId tidak valid, must be positive id", nameof(receiverWalletId));
        }
        if (string.IsNullOrWhiteSpace(type))
        {
            throw new ArgumentException("type is required", nameof(type));
        }
        if(amount <= 0)
        {
            throw new ArgumentException("Amount tidak valid, must be positive value", nameof(amount));
        }

        ReferenceNo = referenceNo;
        ReceiverWalletId = receiverWalletId;
        Type = type;
        Amount = amount;
        Status = "PENDING";
        Description = description;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void ConfirmationSuccess()
    {
        if(Status != "PENDING")
        {
            throw new InvalidOperationException("Hanya Transaction Pending yang dapat di konfirmasi");
        }

        Status = "SUCCESS";
    }
}