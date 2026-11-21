using Ewallet.Infrastructure.Persistence;
using EWallet.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Ewallet.Application.Repositories;

namespace Ewallet.Infrastructure.Repositories;

public sealed class EfPaymentMethodRepository : IPaymentMethod
{
    private readonly AppDbContext _db;
    
    public EfPaymentMethodRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<PaymentMethods>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _db.PaymentMethods.AsNoTracking().Where(pm => pm.IsActive == true).OrderBy(pm => pm.Code).ToArrayAsync(cancellationToken);
    }

    public async Task<PaymentMethods?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _db.PaymentMethods.AsNoTracking().FirstOrDefaultAsync(pm => pm.Id == id, cancellationToken);
    }
}