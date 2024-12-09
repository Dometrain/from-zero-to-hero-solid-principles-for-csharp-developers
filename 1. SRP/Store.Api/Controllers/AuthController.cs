using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Store.Api.Contracts.Requests;
using Store.Common.Helpers;
using Store.Api.Security;

namespace Store.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly TokenService _tokenService;

    public AuthController(TokenService tokenService)
    {
        _tokenService = tokenService.NotNull();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IResult> CreateToken([FromBody] AuthRequest userRequest, CancellationToken cancellationToken = default)
    {
        var validationResult = new Dictionary<string, string[]>();
        if (userRequest == null || string.IsNullOrWhiteSpace(userRequest.Email))
            validationResult.Add("Email", new[] { "Email must not be empty" });
        if (userRequest == null || string.IsNullOrWhiteSpace(userRequest.Password))
            validationResult.Add("Password", new[] { "Password must not be empty" });

        if (validationResult.Any())
            return Results.ValidationProblem(validationResult);

        var token = await _tokenService.CreateTokenAsync(userRequest.Email, userRequest.Password, cancellationToken);
        if (token != null)
            return Results.Ok(token);

        return Results.Unauthorized();
    }
}