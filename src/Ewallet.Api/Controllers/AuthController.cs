using Ewallet.Application.DTOs.Auth;
using Ewallet.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ewallet.Api.Controllers;

[ApiController]
[Route("api/auth")]

public sealed class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthUserResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AuthUserResponse>> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.Register(request, cancellationToken);

        if(result.Status == RegisterStatus.Created)
        {
            return StatusCode(StatusCodes.Status201Created, result.User);
        }

        if (result.Status == RegisterStatus.DuplicateEmail)
        {
            return Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Email Terdaftar",
                Detail = "Email sudah digunakan oleh orang lain"
            });
        }
        var problem = new ValidationProblemDetails(new Dictionary<string, string[]>
        {
            ["registration"] = result.Error
        })
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Registrasion validation failed"
        };

        return BadRequest(problem);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async
    Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var result = await _authService.Login(request, ct);
        if (!result.Succeeded || result.User is null)
        {
        return Unauthorized(new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = "Invalid Access",
            Detail = "Email atau password salah"
        });
        }
        var response = new LoginResponse("Akun berhasil diverifikasi", result.User);
        return Ok(response);
    }
}