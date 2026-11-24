using Ewallet.Application.DTOs;
using Ewallet.Application.DTOs.Dashboard;
using Ewallet.Application.Repositories;
using Ewallet.Infrastructure.Persistence;
using EWallet.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Ewallet.Infrastructure.Repositories;

public sealed class EfUserRepository : IUserRepository
{
    private readonly AppDbContext _db;
    public EfUserRepository(AppDbContext db)
    {
        _db = db;
    }
    public async Task<bool> AddUserWithWalletAsync(Wallets wallets, CancellationToken cancellationToken)
    {
        _db.Wallets.Add(wallets);
        try
        {
            await _db.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateException e) when (e.InnerException is SqlException { Number: 2601 or 2627 })
        {
            _db.Entry(wallets).State = EntityState.Detached;
            return false;
        }
    }

    public async Task<IReadOnlyList<UserResponse>> GetAllAsync(CancellationToken cancellationToken)
    {

        return await _db.Users.AsNoTracking().Select(u => new UserResponse(
            u.Id,
            u.UserName!,
            u.FullName,
            u.Email!,
            u.PhoneNumber!,
            u.Status,
            u.CreatedAtUtc,
            u.UpdatedAtUtc,
            _db.Wallets
                .Where(w => w.AppUserId == u.Id)
                .Select(
                    w => new WalletResponse(
                        w.Id,
                        w.WalletNumber,
                        w.Balance,
                        w.Status
                )).FirstOrDefault()!
        )).ToListAsync(cancellationToken);
    }

    public async Task<UserResponse?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await _db.Users.AsNoTracking().Where(u => u.Id == id).Select(u => new UserResponse(
            u.Id,
            u.UserName!,
            u.FullName,
            u.Email!,
            u.PhoneNumber!,
            u.Status,
            u.CreatedAtUtc,
            u.UpdatedAtUtc,
            _db.Wallets
                .Where(w => w.AppUserId == u.Id)
                .Select(
                    w => new WalletResponse(
                        w.Id,
                        w.WalletNumber,
                        w.Balance,
                        w.Status
                )).FirstOrDefault()!
        )).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<UserResponseDashboard?> GetDashboardAsync(long appUserId, CancellationToken cancellationToken)
    {
        var user = await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(
                u => u.Id == appUserId,
                cancellationToken
            );

        if (user is null)
            return null;

        var wallet = await _db.Wallets
            .AsNoTracking()
            .Include(w => w.WalletMutations)
                .ThenInclude(m => m.Transactions)
            .FirstOrDefaultAsync(
                w => w.AppUserId == appUserId,
                cancellationToken
            );

        if (wallet is null)
            return null;

        var walletResponseDash = new WalletResponseDashboard(
            wallet.Id,
            wallet.WalletNumber,
            wallet.Balance,
            wallet.Status
        );

        var mutations = wallet.WalletMutations
            .OrderByDescending(m => m.CreatedAtUtc)
            .Take(5)
            .Select(m => new WalletMutationResponse(
                m.WalletId,
                m.Direction,
                m.Amount,
                m.BalanceBefore,
                m.BalanceAfter,
                m.CreatedAtUtc
            ))
            .ToList();

        return new UserResponseDashboard(
            user.Id,
            user.UserName ?? string.Empty,
            user.FullName,
            user.Email ?? string.Empty,
            user.PhoneNumber ?? string.Empty,
            user.Status,
            user.CreatedAtUtc,
            user.UpdatedAtUtc,
            walletResponseDash,
            mutations
        );
    }

    public async Task<string?> GetWalletNumberAsync(CancellationToken cancellationToken)
    {
        return await _db.Wallets.OrderByDescending(w => w.WalletNumber).Select(w => w.WalletNumber).FirstOrDefaultAsync(cancellationToken);
    }
}