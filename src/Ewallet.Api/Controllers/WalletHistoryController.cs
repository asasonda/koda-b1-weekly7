using Ewallet.Application.DTOs;
using Ewallet.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ewallet.Api.Controllers;

[ApiController]
[Route("api/wallets")]
public sealed class WalletHistoryController : ControllerBase
{
    private readonly IWalletHistoryService _service;

    public WalletHistoryController(IWalletHistoryService service)
    {
        _service = service;
    }

    [HttpGet("{walletId}/history/transactions")]
    [ProducesResponseType(typeof(PagedResult<TransactionHistoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PagedResult<TransactionHistoryResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(PagedResult<TransactionHistoryResponse>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTransactions([FromRoute] long walletId, [FromQuery] TransactionHistoryRequest request,CancellationToken ct)
    {
        try
        {
            var result = await _service.GetTransactionsAsync(walletId, request, ct);
            if(result is null)
            {
                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Data tidak ditemukan",
                    detail: $"data dengan id {walletId} tidak ditemukan"
                );
            }
            return Ok(new
            {
                message = "Data berhasil diambil",
                data = result
            });
        }
        catch (ArgumentException ex)
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Request tidak valid",
                detail: ex.Message);
        }
    }

    [HttpGet("{walletId}/history/mutations")]
    [ProducesResponseType(typeof(PagedResult<WalletMutationHistoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PagedResult<WalletMutationHistoryResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(PagedResult<WalletMutationHistoryResponse>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetMutations([FromRoute] long walletId, [FromQuery] WalletMutationHistoryRequest request,CancellationToken ct)
    {
        try
        {
            var result = await _service.GetMutationsAsync(walletId, request, ct);

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Request tidak valid",
                detail: ex.Message);
        }
    }
}