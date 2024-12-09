using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Api.Contracts.Requests;
using Store.Api.Mappers;
using Store.Application.Models;
using Store.Application.Services;
using Store.Common.Helpers;
using Store.Common.Results;

namespace Store.Api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize(Roles = Roles.AdminRole)]
public class AdminController : BaseController<User>
{
    private readonly IUserService _userService;
    private readonly IValidator<CreateUserRequest> _createUserValidator;
    public AdminController(IUserService userService, IValidator<CreateUserRequest> createUserValidator)
    {
        _userService = userService.NotNull();
        _createUserValidator = createUserValidator.NotNull();
    }

    [HttpGet("{userId}")]
    public async Task<IResult> GetUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        var result = await _userService.GetUserAsync(userId, cancellationToken);
        return result switch
        {
            SuccessResult<User> successResult => HandleSuccess(successResult.Data?.MapAdmin()),
            NotFoundResult<User> => HandleNotFound(),
            InvalidResult<User> invalidResult => HandleInvalid(invalidResult),
            ErrorResult<User> errorResult => HandleErrors(errorResult),
            _ => HandleUnknown()
        };
    }

    [HttpPost]
    public async Task<IResult> CreateAdminUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _createUserValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var result = await _userService.CreateAdminUserAsync(request.Map(), request.Password, cancellationToken);
        return result switch
        {
            SuccessResult<User> successResult => HandleSuccess(successResult.Data?.Map()),
            NotFoundResult<User> => HandleNotFound(),
            InvalidResult<User> invalidResult => HandleInvalid(invalidResult),
            ErrorResult<User> errorResult => HandleErrors(errorResult),
            _ => HandleUnknown()
        };
    }
}