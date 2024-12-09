using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Store.Api.Contracts.Requests;
using Store.Common.Helpers;
using Store.Api.Security;
using FluentValidation;
using Store.Api.Contracts.Responses;

namespace Store.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly TokenService _tokenService;
    private readonly IValidator<AuthRequest> _userValidator;

    public AuthController(TokenService tokenService, IValidator<AuthRequest> userValidator)
    {
        _tokenService = tokenService.NotNull();
        _userValidator = userValidator.NotNull();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IResult> CreateToken([FromBody] AuthRequest userRequest, CancellationToken cancellationToken = default)
    {
        var validationResult = await _userValidator.ValidateAsync(userRequest, cancellationToken);

        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var token = await _tokenService.CreateTokenAsync(userRequest.Email, userRequest.Password, cancellationToken);
        if (token != null)
            return Results.Ok(token);

        return Results.Unauthorized();
    }
}