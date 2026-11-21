using Ewallet.Application.Repositories;
using Ewallet.Infrastructure.Persistence;
using EWallet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ewallet.Infrastructure.Repositories;

public sealed class EfTransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _db;

    public EfTransactionRepository(AppDbContext db)
    {
        _db = db;
    }
    public async Task<string?> GetReferenceNo(CancellationToken cancellationToken)
    {
        return await _db.Transactions.OrderByDescending(t => t.ReferenceNo).Select(t => t.ReferenceNo).FirstOrDefaultAsync(cancellationToken);
    }
}