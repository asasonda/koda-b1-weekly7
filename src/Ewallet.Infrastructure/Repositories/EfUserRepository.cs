using Ewallet.Application.DTOs;
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


    public async Task<string?> GetWalletNumberAsync(CancellationToken cancellationToken)
    {
        return await _db.Wallets.OrderByDescending(w => w.WalletNumber).Select(w => w.WalletNumber).FirstOrDefaultAsync(cancellationToken);
    }

    // public async Task<IReadOnlyList<Users>> GetAllAsync(CancellationToken cancellationToken)
    // {
    //     return await _db.Users.AsNoTracking().Include(u => u.Wallets).OrderBy(u => u.Id).ToArrayAsync(cancellationToken);
    // }

    // public async Task<Users?> GetByIdAsync(long id, CancellationToken cancellationToken)
    // {
    //     return await _db.Users.AsNoTracking().Include(u => u.Wallets).FirstOrDefaultAsync(u => u.Id == id, cancellationToken: cancellationToken);
    // }

    // public async Task<bool> UpdateUserAsync(Users users, CancellationToken cancellationToken)
    // {
    //     _db.Users.Update(users);

    //     await _db.SaveChangesAsync(cancellationToken);
    //     return true;
    // }
}