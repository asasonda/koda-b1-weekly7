using Ewallet.Application.DTOs.Auth;
using Ewallet.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Ewallet.Api.Controllers;

[ApiController]
[Route("api/auth")]

public sealed class AuthController : ControllerBase
{
    private const string RefreshCookieName = "Ewallet_refresh";
    private readonly IAuthService _authService;
    private readonly IRefreshTokenService _refreshService;
    private readonly IWebHostEnvironment _webEnv;
    private readonly IConfiguration _configuration;
    public AuthController(IAuthService authService, IRefreshTokenService refreshTokenService, IWebHostEnvironment webEnv, IConfiguration configuration)
    {
        _authService = authService;
        _refreshService = refreshTokenService;
        _webEnv = webEnv;
        _configuration = configuration;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    [EnableRateLimiting("auth-register")]
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

    [AllowAnonymous]
    [HttpPost("login")]
    [EnableRateLimiting("auth-login")]
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

        var session = await _refreshService.CreateSessionAsync(result.User, ct);
        WriteRefreshCookie(session.RawRefreshToken, session.RefreshExpiredAtUtc);

        Response.Headers.CacheControl = "no-store";

        var response = new LoginResponse(
            Message: "Akun berhasil diverifikasi", 
            User: session.User,
            AccessToken: session.AccessToken.AccessToken,
            TokenType: "Bearer",
            ExpiresAtUtc: session.AccessToken.ExpiredAtUtc
        );
        return Ok(response);
    }

    [AllowAnonymous]
    [EnableRateLimiting("auth-refresh")]
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<LoginResponse>> Refresh(CancellationToken ct)
    {
        if (!IsTrustedOrigin())
        {
            return StatusCode(StatusCodes.Status403Forbidden, new ProblemDetails
            {
                Status = StatusCodes.Status403Forbidden,
                Title = "Origin not allowed"
            });
        }

        var rawToken = Request.Cookies[RefreshCookieName];
        if (string.IsNullOrWhiteSpace(rawToken))
        {
            return Unauthorized(new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Refresh token not found"
            });
        }

        var session = await _refreshService.RotateAsync(rawToken, ct);
        if (session is null)
        {
            DeleteRefreshCookie();
            return Unauthorized(new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Invalid refresh token",
                Detail = "Sesi tidak valid. Silakan login kembali."
            });
        }

        WriteRefreshCookie(session.RawRefreshToken, session.RefreshExpiredAtUtc);
        Response.Headers.CacheControl = "no-store";

        return Ok(new LoginResponse(
            Message: "Token berhasil diperbarui.",
            User: session.User,
            AccessToken: session.AccessToken.AccessToken,
            TokenType: "Bearer",
            ExpiresAtUtc: session.AccessToken.ExpiredAtUtc));
    }

    [AllowAnonymous]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        if (!IsTrustedOrigin())
        {
            return StatusCode(StatusCodes.Status403Forbidden, new ProblemDetails
            {
                Status = StatusCodes.Status403Forbidden,
                Title = "Origin not allowed"
            });
        }

        var rawToken = Request.Cookies[RefreshCookieName];
        if (!string.IsNullOrWhiteSpace(rawToken))
        {
            await _refreshService.RevokeFamilyAsync(rawToken, cancellationToken);
        }

        DeleteRefreshCookie();

        return NoContent();
    }

    private void WriteRefreshCookie(string rawRefreshToken, DateTimeOffset expiresAtUtc)
    {
        Response.Cookies.Append(RefreshCookieName, rawRefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = !_webEnv.IsDevelopment() || Request.IsHttps,
            SameSite = SameSiteMode.Lax,
            Path = "/api/auth",
            Expires = expiresAtUtc,
            IsEssential = true
        });
    }

    private void DeleteRefreshCookie()
    {
        Response.Cookies.Delete(RefreshCookieName, new CookieOptions
        {
            HttpOnly = true,
            Secure = !_webEnv.IsDevelopment() || Request.IsHttps,
            SameSite = SameSiteMode.Lax,
            Path = "/api/auth",
        });
    }
    private bool IsTrustedOrigin()
    {
        var origin = Request.Headers.Origin.ToString();
        if (string.IsNullOrWhiteSpace(origin))
        {
            return false;
        }
        var allowedOrigins = _configuration
            .GetSection("Cors:AllowedOrigins")
            .Get<string[]>() ?? [];

        return allowedOrigins.Contains(origin, StringComparer.Ordinal);
    }
}