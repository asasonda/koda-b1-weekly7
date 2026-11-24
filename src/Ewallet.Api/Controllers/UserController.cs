using Ewallet.Application.DTOs;
using Ewallet.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ewallet.Api.Controllers;

[ApiController]
[Route("/api/users")]

public sealed class UserController : ControllerBase
{
    private readonly IUserServices _service;

    public UserController(IUserServices services)
    {
        _service = services;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IReadOnlyList<ProblemDetails>), StatusCodes.Status404NotFound)]
    
    public async Task<ActionResult<IReadOnlyList<UserResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var users = await _service.GetAllUserAsync(cancellationToken);
        return Ok(users);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<UserResponse>>> GetById([FromRoute] long id, CancellationToken cancellationToken)
    {
        var users = await _service.GetByUserIdAsync(id, cancellationToken);
        return users is null ? Problem(statusCode: StatusCodes.Status404NotFound, title: "Data tidak ditemukan") : Ok(users);
    }

    

}