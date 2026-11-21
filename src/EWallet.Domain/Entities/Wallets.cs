using System.ComponentModel.DataAnnotations;

namespace EWallet.Domain.Entities;

public sealed class Wallets
{
    private Wallets() {}
    public long Id {get; private set;}

    public long UsersId {get; private set;}
    public Users Users {get; private set;} = null!;
    
    public string WalletNumber {get; private set;}
    public decimal Balance {get; private set;}
    public string Status {get; private set;} = string.Empty;

    [Timestamp]
    public byte[] RowVersion {get; private set;} = null!;
    public DateTime CreatedAtUtc {get; private set;}
    public DateTime? UpdatedAtUtc {get; private set;}


    public ICollection<Transactions> TransactionsSender {get; private set;} = [];
    public ICollection<Transactions> TransactionsReceiver {get; private set;} = [];
    public ICollection<TopUpRequests> TopUpRequests {get; private set;} = [];
    public ICollection<WalletMutations> WalletMutations {get; private set;} = [];


    public Wallets(long usersId, string walletNumber, decimal balance, string status)
    {
        if(usersId <= 0)
        {
            throw new ArgumentException("UsersId is required", nameof(usersId));
        }
        if (string.IsNullOrWhiteSpace(walletNumber))
        {
            throw new ArgumentException("Wallet Number is Required", nameof(walletNumber));
        }
        if(balance < 0)
        {
            throw new ArgumentException("Balance not be negative", nameof(balance));
        }
        if (string.IsNullOrWhiteSpace(status))
        {
            throw new ArgumentException("Status tidak valid", nameof(status));
        }

        UsersId = usersId;
        WalletNumber = walletNumber;
        Balance = balance;
        Status = "ACTIVE";
        CreatedAtUtc = DateTime.UtcNow;
    }


    public Wallets(Users users, string walletNumber,decimal balance)
    {
        if (users is null){
            throw new ArgumentNullException(nameof(users));
        }

        if (string.IsNullOrWhiteSpace(walletNumber)){
            throw new ArgumentException("Wallet Number is Required");
        }

        if (balance < 0){
            throw new ArgumentException("Balance cannot be negative");
        }

        Users = users;
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