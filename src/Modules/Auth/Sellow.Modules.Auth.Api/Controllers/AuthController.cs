using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sellow.Modules.Auth.Core.Features;
using Sellow.Shared.Infrastructure.Api;

namespace Sellow.Modules.Auth.Api.Controllers;

[ApiController]
[Route("/api/v{version:apiVersion}")]
[ApiVersion("1.0")]
internal sealed class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Create a new user.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /users
    ///     {
    ///        "email": "jan@kowalski.pl",
    ///        "username": "jankowalski",
    ///        "password": "p1asw0rd!!
    ///     }
    ///
    /// </remarks>
    /// <response code="201">User has been successfully created.</response>
    /// <response code="400">Request body validation failed.</response>
    /// <response code="409">User with given credentials already exists.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost("users")]
    [ProducesResponseType(201)]
    public async Task<IResult> CreateUser([FromBody] CreateUser command, CancellationToken cancellationToken)
    {
        var userId = await _mediator.Send(command, cancellationToken);

        return Results.Created($"{Request.GetActionFullUrlPath()}/{userId}", null);
    }
}