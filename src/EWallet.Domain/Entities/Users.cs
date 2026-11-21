namespace EWallet.Domain.Entities;

public sealed class Users
{
    private Users() {}
    public long Id {get; private set;}
    public string UserName {get; private set;}
    public string FullName {get; private set;}
    public string Email {get; private set;}
    public string Phone {get; private set;}
    public string? PasswordHash {get; private set;}
    public string Status {get; private set;}
    public DateTime CreatedAtUtc {get; private set;}
    public DateTime? UpdatedAtUtc {get; private set;}


    public Wallets Wallets {get; private set;} = null!;
    public ICollection<AuditLogs> AuditLogs { get; private set; } = [];
    

    public Users(string userName, string fullName, string email, string phone, string? passwordHash)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            throw new ArgumentException("Username not valid", nameof(userName));
        }
        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ArgumentException("fullname not valid", nameof(fullName));
        }
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email is required", nameof(email));
        }

        if (string.IsNullOrWhiteSpace(phone))
        {
            throw new ArgumentException("Phone is required", nameof(phone));
        }

        UserName = userName;
        FullName = fullName;
        Email = email;
        Phone = phone;
        PasswordHash = passwordHash;
        Status = "ACTIVE";
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void UpdateDetail(string userName, string fullName, string email, string phone, string? passwordHash)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            throw new ArgumentException("Username not valid", nameof(userName));
        }
        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ArgumentException("fullname not valid", nameof(fullName));
        }
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email is required", nameof(email));
        }

        if (string.IsNullOrWhiteSpace(phone))
        {
            throw new ArgumentException("Phone is required", nameof(phone));
        }

        UserName = userName;
        FullName = fullName;
        Email = email;
        Phone = phone;
        PasswordHash = passwordHash;
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