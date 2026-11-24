using Ewallet.Application.Services;
using KodaStore.Application.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Ewallet.Api.Controllers.Dashboard;

[ApiController]
[Route("api/dashboard")]
[Authorize(Policy = AppPolicies.CanManageWallet)]
public sealed class DashboardController(IUserServices services) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetDashboard(CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
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