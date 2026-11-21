using Ewallet.Application.DTOs;
using Ewallet.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ewallet.Api.Controllers;

[ApiController]
[Route("/api/wallets")]
public sealed class WalletController : ControllerBase
{
    private readonly IWalletServices _service;
    private readonly ITopUpRequestServices _topUpService;

    public WalletController(IWalletServices services, ITopUpRequestServices topUpService)
    {
        _service = services;
        _topUpService = topUpService;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(WalletResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WalletResponse>> GetByWalletId([FromRoute] long id, CancellationToken cancellationToken)
    {
        var wallets = await _service.GetByIdWalletAsync(id, cancellationToken);
        return wallets is null ? Problem(statusCode: StatusCodes.Status404NotFound, title:"Data tidak ditemukan") : Ok(wallets);
    }


    
    [HttpPost("{walletId}/topups")]
    [ProducesResponseType(typeof(TransactionsTopUp), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> TopUp([FromRoute] long walletId, [FromBody] TopUpRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _service.TopUpAsync(walletId, request, cancellationToken);
            if (result is null)
            {
                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Wallet tidak ditemukan",
                    detail: $"Wallet untuk user ID {walletId} tidak ditemukan."
                );
            }

            return Ok(new
            {
                message = "Top up Created",
                data = result
            });
        }
        catch (ArgumentException ex)
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Top up gagal",
                detail: ex.Message
            );
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

    [HttpGet("{walletId}/topups/detail")]
    [ProducesResponseType(typeof(PaymentMethodResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PaymentMethodResponse>> GetByITopUp([FromRoute] long walletId, CancellationToken cancellationToken)
    {
        var topUps = await _topUpService.GetByWalletIdTopUpAsync(walletId, cancellationToken);
        return topUps is null ? Problem(statusCode: StatusCodes.Status404NotFound, title: "data tidak ditemukan") : Ok(topUps);
    }

    [HttpPatch("{id}/topups/confirm")]
    [ProducesResponseType(typeof(TopUpResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TopUpResponse>> Confirm([FromRoute] long id, CancellationToken cancellationToken)
    {
        try
        {
            var confirm = await _topUpService.ConfirmTopUpAsync(id, cancellationToken);
            if(confirm is null)
            {
                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Data topup tidak ditemukan",
                    detail: $"Wallet dengan topUp id {id} tidak ditemukan"
                );
            }

            return Ok(new
            {
                message = "Top Up Success",
                data = confirm
            });
        }
        catch (ArgumentException ex)
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Top up belum di confirm",
                detail: ex.Message
            );
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