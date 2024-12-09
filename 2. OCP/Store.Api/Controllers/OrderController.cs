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

    public OrderController(
        IOrderService orderService
    )
    {
        _orderService = orderService.NotNull();
    }

    [HttpGet]
    public async Task<IResult> GetOrdersAsync([FromQuery] PagedRequest request, CancellationToken cancellationToken = default)
    {
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
