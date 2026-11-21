using Ewallet.Application.Repositories;
using Ewallet.Infrastructure.Persistence;
using EWallet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ewallet.Infrastructure.Repositories;

public sealed class EfWalletRepository : IWalletRepository
{
    private readonly AppDbContext _db;

    public EfWalletRepository(AppDbContext db)
    {
        _db = db;
    }
    public async Task<bool> AddBalanceAsync(long id, decimal amount, CancellationToken cancellationToken)
    {
        var wallet = await _db.Wallets.AsNoTracking().FirstOrDefaultAsync(w => w.Id == id, cancellationToken);

        if(wallet is null)
        {
            return false;
        }

        wallet.AddBalance(amount);

        return true;
    }

    public async Task<bool> CreateTopUp(Transactions transactions, TopUpRequests topUpRequests, CancellationToken cancellationToken)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            _db.Transactions.Add(transactions);
            _db.TopUpRequests.Add(topUpRequests);
            await _db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return true;
        }
        catch (System.Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            _db.Entry(transactions).State = EntityState.Detached;
            _db.Entry(topUpRequests).State = EntityState.Detached;
            return false;
        }
    }
    
    public async Task<Wallets?> GetByIdWallet(long walletId, CancellationToken cancellationToken)
    {
        return await _db.Wallets.FirstOrDefaultAsync(w => w.Id == walletId, cancellationToken);
    }

    public async Task<TopUpRequests?> GetByWalletIdTopUp(long id, CancellationToken cancellationToken)
    {
        return await _db.TopUpRequests.AsNoTracking().Include(tp => tp.PaymentMethods).FirstOrDefaultAsync(tp => tp.WalletId == id, cancellationToken);
    }

    public async Task<bool> ReduceBalanceAsync(long id, decimal amount, CancellationToken cancellationToken)
    {
        var wallet = await _db.Wallets.AsNoTracking().FirstOrDefaultAsync(w => w.Id == id, cancellationToken);

        if(wallet is null)
        {
            return false;
        }
        wallet.ReduceBalance(amount);
        return true;
    }
}