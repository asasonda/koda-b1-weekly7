using Ewallet.Application.DTOs;

namespace Ewallet.Application.Services;

public interface IPaymentMethodServices
{
    Task<IReadOnlyList<PaymentMethodDetailsResponse>> GetAllPaymentMethodAsync(CancellationToken cancellationToken);
}