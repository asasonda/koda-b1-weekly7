using Ewallet.Application.DTOs;
using Ewallet.Application.Repositories;

namespace Ewallet.Application.Services;

public sealed class PaymentMethodServices : IPaymentMethodServices
{
    private readonly IPaymentMethod _repository;

    public PaymentMethodServices(IPaymentMethod repository)
    {
        _repository = repository;
    }
    public async Task<IReadOnlyList<PaymentMethodDetailsResponse>> GetAllPaymentMethodAsync(CancellationToken cancellationToken)
    {
        var method = await _repository.GetAllAsync(cancellationToken);

        return method.Select(m => new PaymentMethodDetailsResponse(
            m.Id,
            m.Code,
            m.Name,
            m.IsActive
        )).ToArray();
    }
}