using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Api.Contracts.Requests;
using Store.Api.Mappers;
using Store.Application.Models;
using Store.Common.Results;
using Store.Application.Services;
using Store.Common.Helpers;
using FluentValidation;

namespace Store.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class OrderController : BaseController<Order>
{
    private readonly IOrderService _orderService;
    private readonly IValidator<PagedRequest> _pagedValidator;
    private readonly IValidator<OrderRequest> _orderValidator;

    public OrderController(
        IOrderService orderService,
        IValidator<PagedRequest> pagedValidator,
        IValidator<OrderRequest> orderValidator
    )
    {
        _orderService = orderService.NotNull();
        _pagedValidator = pagedValidator.NotNull();
        _orderValidator = orderValidator.NotNull();
    }

    [HttpGet]
    public async Task<IResult> GetOrdersAsync([FromQuery] PagedRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _pagedValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var result = await _orderService.GetOrdersAsync(UserId, request.Page, request.PageSize, cancellationToken);
        return result switch
        {
            SuccessResult<Paged<Order>> successResult => HandleSuccess(successResult.Data?.Map()),
            NotFoundResult<Paged<Order>> => HandleNotFound(),
            _ => HandleUnknown()
        };
    }

    [HttpGet("{orderId}")]
    public async Task<IResult> GetOrderAsync(int orderId, CancellationToken cancellationToken = default)
    {
        var result = await _orderService.GetOrderAsync(UserId, orderId, cancellationToken);
        return result switch
        {
            SuccessResult<Order> successResult => HandleSuccess(successResult.Data?.Map()),
            NotFoundResult<Order> => HandleNotFound(),
            InvalidResult<Order> invalidResult => HandleInvalid(invalidResult),
            ErrorResult<Order> errorResult => HandleErrors(errorResult),
            _ => HandleUnknown()
        };
    }

    [HttpPost]
    public async Task<IResult> CreateOrderAsync([FromBody] OrderRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _orderValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var result = await _orderService.CreateOrderAsync(UserId, request.CartId, cancellationToken);
        return result switch
        {
            SuccessResult<Order> successResult => HandleSuccess(successResult.Data?.Map()),
            NotFoundResult<Order> => HandleNotFound(),
            InvalidResult<Order> invalidResult => HandleInvalid(invalidResult),
            ErrorResult<Order> errorResult => HandleErrors(errorResult),
            _ => HandleUnknown()
        };
    }
}
