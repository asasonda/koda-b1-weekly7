using Ewallet.Application.DTOs;
using Ewallet.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ewallet.Api.Controllers;

[ApiController]
[Route("api/transfer")]
public sealed class TransferController : ControllerBase
{
    private readonly ITransferService _service;

    public TransferController(ITransferService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType(typeof(TransactionsTransferResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Transfer([FromBody] TransferRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _service.TransferAsync(request, cancellationToken);
            if (result is null)
            {
                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "Wallet tidak ditemukan",
                    detail: "Wallet pengirim atau penerima tidak ditemukan."
                );
            }
            return Ok(new
            {
                message = "Transfer berhasil",
                data = result
            });
        }
        catch (ArgumentException ex)
        {
            return Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Transfer gagal",
                detail: ex.Message
            );
        }
        catch (KeyNotFoundException ex)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Wallet tidak ditemukan",
                detail: ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Problem(
                statusCode: StatusCodes.Status409Conflict,
                title: "Tidak dapat transfer ke diri sendiri",
                detail: ex.Message);
        }
    }
}

