namespace EWallet.Domain.Entities;

public sealed class Users
{
    private Users() {}
    public long Id {get; private set;}
    public long AppUserId { get; private set; }
    public string FullName {get; private set;}
    public string Status {get; private set;}
    public DateTime CreatedAtUtc {get; private set;}
    public DateTime? UpdatedAtUtc {get; private set;}


    public Wallets Wallets {get; private set;} = null!;
    public ICollection<AuditLogs> AuditLogs { get; private set; } = [];
    

    public Users(long appUserId, string fullName)
    {
        if(appUserId <= 0) throw new ArgumentException("Appuserid is required");
        if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("fullname not valid", nameof(fullName));

        AppUserId = appUserId;
        FullName = fullName;
        Status = "ACTIVE";
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void UpdateDetail(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ArgumentException("fullname not valid", nameof(fullName));
        }

        FullName = fullName;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Suspend()
    {
        Status = "SUSPENDED";
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Activate()
    {
        Status = "ACTIVE";
        UpdatedAtUtc = DateTime.UtcNow;
    }
}