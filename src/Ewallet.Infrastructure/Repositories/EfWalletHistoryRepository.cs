using Ewallet.Infrastructure.Persistence;
using Ewallet.Application.DTOs;
using EWallet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Ewallet.Application.Repositories;

namespace Ewallet.Infrastructure.Repositories;

public sealed class EfWalletHistoryRepository(AppDbContext db)
    : IWalletHistoryRepository
{
    private readonly AppDbContext _db = db;

    public async Task<PagedResult<TransactionHistoryResponse>> GetTransactionsAsync(long walletId, TransactionHistoryCriteria criteria,CancellationToken ct)
    {
        IQueryable<Transactions> query = _db.Transactions
            .AsNoTracking()
            .Where(t => t.SenderWalletId == walletId || t.ReceiverWalletId == walletId);

        if (criteria.ReferenceNo is { Length: > 0 } referenceNo)
        {
            query = query.Where(t => t.ReferenceNo.Contains(referenceNo));
        }

        if (criteria.Type is { Length: > 0 } type)
        {
            query = query.Where(t => t.Type == type);
        }

        if (criteria.Status is { Length: > 0 } status)
        {
            query = query.Where(t => t.Status == status);
        }

        if (criteria.FromDate is DateTime fromDate)
        {
            query = query.Where(t => t.CreatedAtUtc >= fromDate);
        }

        if (criteria.ToDate is DateTime toDate)
        {
            query = query.Where(t => t.CreatedAtUtc <= toDate);
        }

        long totalCount = await query.LongCountAsync(ct);

        IOrderedQueryable<Transactions> sorted = criteria.Sort switch
        {
            "date_asc" => query.OrderBy(t => t.CreatedAtUtc).ThenBy(t => t.Id),
            "date_desc" => query.OrderByDescending(t => t.CreatedAtUtc).ThenByDescending(t => t.Id),
            "amount_asc" => query.OrderBy(t => t.Amount).ThenBy(t => t.Id),
            "amount_desc" => query.OrderByDescending(t => t.Amount).ThenByDescending(t => t.Id),

            _ => throw new ArgumentOutOfRangeException(nameof(criteria.Sort))
        };

        int skip = (criteria.Page - 1) * criteria.PageSize;

        var items = await sorted
            .Skip(skip)
            .Take(criteria.PageSize)
            .Select(t => new TransactionHistoryResponse(
                t.Id,
                t.ReferenceNo,
                t.SenderWalletId,
                t.ReceiverWalletId,
                t.Type,
                t.Amount,
                t.Status,
                t.Description,
                t.CreatedAtUtc,
                t.CompletedAtUtc
            ))
            .ToArrayAsync(ct);

        long totalPages =
            (totalCount / criteria.PageSize) +
            (totalCount % criteria.PageSize == 0 ? 0 : 1);

        return new PagedResult<TransactionHistoryResponse>(
            criteria.Page,
            criteria.PageSize,
            totalCount,
            totalPages,
            items
        );
    }

    public async Task<PagedResult<WalletMutationHistoryResponse>> GetMutationsAsync(long walletId,WalletMutationHistoryCriteria criteria,CancellationToken ct)
    {
        IQueryable<WalletMutations> query = _db.WalletMutations
            .AsNoTracking()
            .Where(m => m.WalletId == walletId);

        if (criteria.ReferenceNo is { Length: > 0 } referenceNo)
        {
            query = query.Where(m => m.Transactions.ReferenceNo.Contains(referenceNo));
        }

        if (criteria.Direction is { Length: > 0 } direction)
        {
            query = query.Where(m => m.Direction == direction);
        }

        if (criteria.FromDate is DateTime fromDate)
        {
            query = query.Where(m => m.CreatedAtUtc >= fromDate);
        }

        if (criteria.ToDate is DateTime toDate)
        {
            query = query.Where(m => m.CreatedAtUtc <= toDate);
        }

        long totalCount = await query.LongCountAsync(ct);

        IOrderedQueryable<WalletMutations> sorted = criteria.Sort switch
        {
            "date_asc" => query.OrderBy(m => m.CreatedAtUtc).ThenBy(m => m.Id),
            "date_desc" => query.OrderByDescending(m => m.CreatedAtUtc).ThenByDescending(m => m.Id),
            "amount_asc" => query.OrderBy(m => m.Amount).ThenBy(m => m.Id),
            "amount_desc" => query.OrderByDescending(m => m.Amount).ThenByDescending(m => m.Id),
            _ => throw new ArgumentOutOfRangeException(nameof(criteria.Sort))
        };

        int skip = (criteria.Page - 1) * criteria.PageSize;

        var items = await sorted
            .Skip(skip)
            .Take(criteria.PageSize)
            .Select(m => new WalletMutationHistoryResponse(
                m.Id,
                m.Transactions.ReferenceNo,
                m.WalletId,
                m.Direction,
                m.Amount,
                m.BalanceBefore,
                m.BalanceAfter,
                m.CreatedAtUtc
            ))
            .ToArrayAsync(ct);

        long totalPages =
            (totalCount / criteria.PageSize) +
            (totalCount % criteria.PageSize == 0 ? 0 : 1);

        return new PagedResult<WalletMutationHistoryResponse>(
            criteria.Page,
            criteria.PageSize,
            totalCount,
            totalPages,
            items
        );
    }

    public async Task<bool> WalletExistsAsync(long walletId, CancellationToken ct)
    {
        return await _db.Wallets
            .AsNoTracking()
            .AnyAsync(w => w.Id == walletId, ct);
    }
}