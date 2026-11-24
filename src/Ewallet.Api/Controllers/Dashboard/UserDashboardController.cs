using Ewallet.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ewallet.Api.Controllers;

[ApiController]
[Route("api/dashboard")]
[Authorize(Roles = "CUSTOMER")]
public sealed class DashboardController(IUserServices services) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetDashboard(CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!long.TryParse(userIdClaim, out var appUserId))
            return Unauthorized();

        var result = await services.GetDashboardAsync(
            appUserId,
            cancellationToken
        );

        if (result is null)
            return NotFound(new
            {
                message = "Dashboard data tidak ditemukan."
            });

        return Ok(result);
    }
}