using System.ComponentModel.DataAnnotations;

namespace EWallet.Domain.Entities;

public sealed class Wallets
{
    private Wallets() {}
    public long Id {get; private set;}
    public long AppUserId {get; private set;}    
    public string WalletNumber {get; private set;}
    public decimal Balance {get; private set;}
    public string Status {get; private set;} = string.Empty;
    public byte[] RowVersion {get; private set;} = null!;
    public DateTime CreatedAtUtc {get; private set;}
    public DateTime? UpdatedAtUtc {get; private set;}


    public ICollection<Transactions> TransactionsSender {get; private set;} = [];
    public ICollection<Transactions> TransactionsReceiver {get; private set;} = [];
    public ICollection<TopUpRequests> TopUpRequests {get; private set;} = [];
    public ICollection<WalletMutations> WalletMutations {get; private set;} = [];


    public Wallets(long appUserId, string walletNumber, decimal balance)
    {
        if(appUserId <= 0)
        {
            throw new ArgumentException("UsersId is required", nameof(appUserId));
        }
        if (string.IsNullOrWhiteSpace(walletNumber))
        {
            throw new ArgumentException("Wallet Number is Required", nameof(walletNumber));
        }
        if(balance < 0)
        {
            throw new ArgumentException("Balance not be negative", nameof(balance));
        }

        AppUserId = appUserId;
        WalletNumber = walletNumber;
        Balance = balance;
        Status = "ACTIVE";
        CreatedAtUtc = DateTime.UtcNow;
    }


    public void AddBalance(decimal amount)
    {
        if(amount <= 0)
        {
            throw new ArgumentException("Amount tidak valid", nameof(amount));
        }

        Balance += amount;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void ReduceBalance(decimal amount)
    {
        if(amount <= 0)
        {
            throw new ArgumentException("Amount tidak valid", nameof(amount));
        }

        if(Balance < amount)
        {
            throw new ArgumentException("Saldo tidak mencukupi");
        }

        Balance -= amount;
        UpdatedAtUtc = DateTime.UtcNow;
    }
}