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
    public async Task<bool> AddUserWithWalletAsync(Users users, Wallets wallets, CancellationToken cancellationToken)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            _db.Users.Add(users);
            _db.Wallets.Add(wallets);
            await _db.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateException e) when (e.InnerException is SqlException { Number: 2601 or 2627 })
        {
            await transaction.RollbackAsync(cancellationToken);

            _db.Entry(users).State = EntityState.Detached;
            _db.Entry(wallets).State = EntityState.Detached;
            return false;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<string?> GetWalletNumberAsync(CancellationToken cancellationToken)
    {
        return await _db.Wallets.AsNoTracking().OrderByDescending(w => w.WalletNumber).Select(w => w.WalletNumber).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Users>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _db.Users.AsNoTracking().Include(u => u.Wallets).OrderBy(u => u.Id).ToArrayAsync(cancellationToken);
    }

    public async Task<Users?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await _db.Users.AsNoTracking().Include(u => u.Wallets).FirstOrDefaultAsync(u => u.Id == id, cancellationToken: cancellationToken);
    }

    public async Task<bool> UpdateUserAsync(Users users, CancellationToken cancellationToken)
    {
        _db.Users.Update(users);

        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}