using EWallet.Domain.Entities;

namespace Ewallet.Application.Repositories;

public interface IPaymentMethod
{
    Task<PaymentMethods?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<IReadOnlyList<PaymentMethods>> GetAllAsync(CancellationToken cancellationToken);
}