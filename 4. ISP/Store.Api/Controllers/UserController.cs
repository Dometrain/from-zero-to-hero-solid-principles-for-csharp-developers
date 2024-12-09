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
public class UserController : BaseController<User>
{
    private readonly IUserService _userService;
    public UserController(
        IUserService userService
    )
    {
        _userService = userService.NotNull();
    }

    [HttpGet]
    [Authorize]
    public async Task<IResult> GetUserAsync(CancellationToken cancellationToken = default)
    {
        var result = await _userService.GetUserAsync(UserId, cancellationToken);
        return result switch
        {
            SuccessResult<User> successResult => HandleSuccess(successResult.Data?.Map()),
            NotFoundResult<User> => HandleNotFound(),
            InvalidResult<User> invalidResult => HandleInvalid(invalidResult),
            ErrorResult<User> errorResult => HandleErrors(errorResult),
            _ => HandleUnknown()
        };
    }

    [HttpPost]
    public async Task<IResult> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _userService.CreateUserAsync(request.Map(), request.Password, cancellationToken);
        return result switch
        {
            SuccessResult<User> successResult => HandleSuccess(successResult.Data?.Map()),
            NotFoundResult<User> => HandleNotFound(),
            InvalidResult<User> invalidResult => HandleInvalid(invalidResult),
            ErrorResult<User> errorResult => HandleErrors(errorResult),
            _ => HandleUnknown()
        };
    }

    [HttpPut]
    [Authorize]
    public async Task<IResult> UpdateUserAsync(UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _userService.UpdateUserAsync(UserId, request.Map(), cancellationToken);
        return result switch
        {
            SuccessResult<User> successResult => HandleSuccess(successResult.Data?.Map()),
            NotFoundResult<User> => HandleNotFound(),
            InvalidResult<User> invalidResult => HandleInvalid(invalidResult),
            ErrorResult<User> errorResult => HandleErrors(errorResult),
            _ => HandleUnknown()
        };
    }

    [HttpPut("password")]
    [Authorize]
    public async Task<IResult> UpdatePasswordAsync(UpdatePasswordRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _userService.UpdatePasswordAsync(UserId, request.Password, cancellationToken);
        return result switch
        {
            SuccessResult => Results.Ok(),
            ErrorResult errorResult => HandleErrors(errorResult),
            _ => HandleUnknown()
        };
    }
}