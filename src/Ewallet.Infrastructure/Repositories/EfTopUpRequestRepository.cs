using Ewallet.Infrastructure.Persistence;
using Ewallet.Application.DTOs;
using EWallet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Ewallet.Application.Repositories;

namespace Ewallet.Infrastructure.Repositories;

public sealed class EfTopUpRequestRespository : ITopUpRequest
{
    private readonly AppDbContext _db;
    
    public EfTopUpRequestRespository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<bool> ConfirmAsync(long id, CancellationToken cancellationToken)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var topUp = await _db.TopUpRequests.FirstOrDefaultAsync(tp => tp.Id == id, cancellationToken);
            
            if(topUp is null || topUp.Status != "PENDING")
            {
                return false;
            }

            var transactionTopUp = await _db.Transactions.FirstOrDefaultAsync(t => t.Id == topUp.TransactionId, cancellationToken);
            if(transactionTopUp is null || transactionTopUp.Status != "PENDING")
            {
                return false;
            }

            var wallet = await _db.Wallets.FirstOrDefaultAsync(w => w.Id == topUp.WalletId, cancellationToken);
            if(wallet is null)
            {
                return false;
            }

            var balanceBefore = wallet.Balance;
            
            wallet.AddBalance(topUp.Amount);

            var balanceAfter = wallet.Balance;

            topUp.ConfirmationSuccess();
            transactionTopUp.ConfirmationSuccess();

            var walletMutation = new WalletMutations(
                transactionTopUp.Id,
                wallet.Id,
                "IN",
                topUp.Amount,
                balanceBefore,
                balanceAfter
            );

            // var auditLog = new AuditLogs(
            //     wallet.UsersId,
            //     transactionTopUp.Id,
            //     "Confimation TopUp",
            //     "TopUpRequests",
            //     topUp.Id.ToString(),
            //     transactionTopUp.Status,
            //     "Topup successfuly",
            //     null
            // );

            _db.WalletMutations.Add(walletMutation);
            // _db.AuditLogs.Add(auditLog);

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

    public async Task<TopUpRequests?> GetByIdTopUp(long id, CancellationToken cancellationToken)
    {
        return await _db.TopUpRequests.AsNoTracking().Include(tp => tp.PaymentMethods).FirstOrDefaultAsync(tp => tp.Id == id, cancellationToken);
    }

    public async Task<TopUpRequests?> GetByWalledIdTopUp(long walletId, CancellationToken cancellationToken)
    {
        return await _db.TopUpRequests.AsNoTracking().Include(tp => tp.PaymentMethods).FirstOrDefaultAsync(tp => tp.WalletId == walletId, cancellationToken);
    }
}