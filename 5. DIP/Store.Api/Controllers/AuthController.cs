using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Store.Api.Contracts.Requests;
using Store.Api.Security;
using Store.Api.Contracts.Validators;

namespace Store.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly TokenService _tokenService;

    public AuthController()
    {
        _tokenService = new TokenService();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IResult> CreateToken([FromBody] AuthRequest userRequest, CancellationToken cancellationToken = default)
    {
        var token = await _tokenService.CreateTokenAsync(userRequest.Email, userRequest.Password, cancellationToken);
        if (token != null)
            return Results.Ok(token);

        return Results.Unauthorized();
    }
}