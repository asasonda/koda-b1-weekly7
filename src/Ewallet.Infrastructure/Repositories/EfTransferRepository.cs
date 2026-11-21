using Ewallet.Application.Repositories;
using Ewallet.Infrastructure.Persistence;
using EWallet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ewallet.Infrastructure.Repositories;

public sealed class EfTransferRepository : ITransferRepository
{
    private readonly AppDbContext _db;

    public EfTransferRepository(AppDbContext db)
    {
        _db = db;
    }
    public async Task<bool> TransferAsync(Transactions transactions, WalletMutations senderMutation, WalletMutations receiverMutation, CancellationToken cancellationToken)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _db.Transactions.Add(transactions);
            _db.WalletMutations.Add(senderMutation);
            _db.WalletMutations.Add(receiverMutation);
            await _db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return true;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}