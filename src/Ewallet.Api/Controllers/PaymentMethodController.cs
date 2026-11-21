using Ewallet.Application.DTOs;
using Ewallet.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ewallet.Api.Controllers;

[ApiController]
[Route("api/payment-method")]

public sealed class PaymentMethodController : ControllerBase
{
    private readonly IPaymentMethodServices _services;

    public PaymentMethodController(IPaymentMethodServices services)
    {
        _services = services;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<PaymentMethodDetailsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IReadOnlyList<ProblemDetails>), StatusCodes.Status404NotFound)]

    public async Task<ActionResult<IReadOnlyList<PaymentMethodDetailsResponse>>> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var method = await _services.GetAllPaymentMethodAsync(cancellationToken);
            if(method is null)
            {
                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Payment tidak tersedia saat ini"
                );
            }
            return Ok(new
            {
                message = "Berhasil Get data",
                data = method
            });
            
        }
        catch (Exception)
        {
            return Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal Server Error",
                detail: "Terjadi kesalahan saat memproses top up."
            );
        }
    }
}