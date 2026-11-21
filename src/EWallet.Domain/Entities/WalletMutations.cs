namespace EWallet.Domain.Entities;

public sealed class WalletMutations
{
    private WalletMutations() {}

    public long Id {get; private set;}

    public long TransactionsId {get; private set;}
    public Transactions Transactions {get; private set;} = null!;

    public long WalletId {get; private set;}
    public Wallets Wallets {get; private set;} = null!;

    public string Direction {get; private set;}
    public decimal Amount {get; private set;}
    public decimal BalanceBefore {get; private set;}
    public decimal BalanceAfter {get; private set;}
    public DateTime CreatedAtUtc {get; private set;} 

    public WalletMutations(long transactionId, long walletId, string direction, decimal amount, decimal balanceBefore, decimal balanceAfter)
    {
        if(transactionId < 0)
        {
            throw new ArgumentException("transactionId harus positive", nameof(transactionId));
        }
        if(walletId < 0)
        {
            throw new ArgumentException("Wallet id harus positive", nameof(walletId));
        }
        if(direction.ToUpper() != "IN" && direction.ToUpper() != "OUT")
        {
            throw new ArgumentException("Tidak sesuai format", nameof(direction));
        }
        if(amount <= 0)
        {
            throw new ArgumentException("Amount harus positiv", nameof(amount));
        }
        if(balanceBefore < 0)
        {
            throw new ArgumentException("Balance harus angka positiv", nameof(balanceBefore));
        }
        if(balanceAfter < 0)
        {
            throw new ArgumentException("Balance harus angka positif", nameof(balanceAfter));
        }
        
        TransactionsId = transactionId;
        WalletId = walletId;
        Direction = direction;
        Amount = amount;
        BalanceBefore = balanceBefore;
        BalanceAfter = balanceAfter;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public WalletMutations(Transactions transactions, long walletId, string direction, decimal amount, decimal balanceBefore, decimal balanceAfter)
    {
        if(walletId < 0)
        {
            throw new ArgumentException("Wallet id harus positive", nameof(walletId));
        }
        if(direction.ToUpper() != "IN" && direction.ToUpper() != "OUT")
        {
            throw new ArgumentException("Tidak sesuai format", nameof(direction));
        }
        if(amount <= 0)
        {
            throw new ArgumentException("Amount harus positiv", nameof(amount));
        }
        if(balanceBefore < 0)
        {
            throw new ArgumentException("Balance harus angka positiv", nameof(balanceBefore));
        }
        if(balanceAfter < 0)
        {
            throw new ArgumentException("Balance harus angka positif", nameof(balanceAfter));
        }
        
        Transactions = transactions;
        WalletId = walletId;
        Direction = direction;
        Amount = amount;
        BalanceBefore = balanceBefore;
        BalanceAfter = balanceAfter;
        CreatedAtUtc = DateTime.UtcNow;
    }


}