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
    private readonly IOrderService _orderService;
    private readonly IValidator<CreateUserRequest> _createUserValidator;
    public AdminController(
        IUserService userService,
        IOrderService orderService,
        IValidator<CreateUserRequest> createUserValidator
    )
    {
        _userService = userService.NotNull();
        _orderService = orderService.NotNull();
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

    [HttpGet("report")]
    public async Task<IResult> GetReportAsync([FromQuery] string from, [FromQuery] string to, [FromQuery] string interval, CancellationToken cancellationToken = default)
    {
        if (!DateTime.TryParse(from, out DateTime fromDate))
            fromDate = DateTime.UtcNow.AddDays(-30);
        if (!DateTime.TryParse(to, out DateTime toDate))
            toDate = DateTime.UtcNow;
        if (!Enum.TryParse(interval, out ReportInterval reportInterval))
            reportInterval = ReportInterval.Day;

        var result = await _orderService.GetOrderReportAsync(fromDate, toDate, reportInterval, cancellationToken);
        return Results.Ok(result.Data.Map());
    }
}