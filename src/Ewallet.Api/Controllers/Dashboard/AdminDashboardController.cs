using Ewallet.Application.Services;
using KodaStore.Application.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Ewallet.Api.Controllers.Dashboard;

[ApiController]
[Route("api/admin")]
[Authorize(Policy = AppPolicies.CanManageUser)]
public sealed class AdminDashboardController(IUserServices services) : ControllerBase
{
    [HttpGet("dashboard")]
    public async Task<IActionResult> GetUserDashboard(CancellationToken cancellationToken)
    {
        var result = await services.GetAdminDashboardAsync(cancellationToken);
        return Ok(result);
    }
}