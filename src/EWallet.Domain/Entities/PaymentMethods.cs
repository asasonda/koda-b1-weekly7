namespace EWallet.Domain.Entities;

public sealed class PaymentMethods
{
    private PaymentMethods() {}

    public int Id {get; private set;}
    public string Code {get; private set;}
    public string Name {get; private set;}
    public string? ProviderName {get; private set;}
    public bool IsActive {get; private set;}
    public DateTime CreatedAtUtc {get; private set;}

    public ICollection<TopUpRequests> TopUpRequests {get; private set;} = [];

    public PaymentMethods(string code, string name, string? providerName, bool isActive)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Code tidak valid", nameof(code));
        }
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name tidak valid", nameof(name));
        }

        Code = code;
        Name = name;
        ProviderName = providerName;
        IsActive = isActive;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public PaymentMethods(string code, string name, bool isActive)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Code tidak valid", nameof(code));
        }
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name tidak valid", nameof(name));
        }

        Code = code;
        Name = name;
        IsActive = isActive;
        CreatedAtUtc = DateTime.Now;
    }
}