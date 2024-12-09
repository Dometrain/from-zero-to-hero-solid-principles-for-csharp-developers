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
public class CartController : BaseController<Cart>
{
    private readonly ICartService _cartService;

    public CartController(
        ICartService cartService
    )
    {
        _cartService = cartService.NotNull();
    }

    [HttpGet("{cartId}")]
    public async Task<IResult> GetCartAsync(int cartId, CancellationToken cancellationToken = default)
    {
        var result = await _cartService.GetCartAsync(UserId, cartId, cancellationToken);
        return result switch
        {
            SuccessResult<Cart> successResult => HandleSuccess(successResult.Data?.Map()),
            NotFoundResult<Cart> => HandleNotFound(),
            InvalidResult<Cart> invalidResult => HandleInvalid(invalidResult),
            ErrorResult<Cart> errorResult => HandleErrors(errorResult),
            _ => HandleUnknown()
        };
    }

    [HttpPost]
    public async Task<IResult> CreateCartAsync(CartRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _cartService.CreateCartAsync(UserId, request.Map(), cancellationToken);
        return result switch
        {
            SuccessResult<Cart> successResult => HandleSuccess(successResult.Data?.Map()),
            NotFoundResult<Cart> => HandleNotFound(),
            InvalidResult<Cart> invalidResult => HandleInvalid(invalidResult),
            ErrorResult<Cart> errorResult => HandleErrors(errorResult),
            _ => HandleUnknown()
        };
    }

    [HttpPost("{cartId}/items")]
    public async Task<IResult> AddToCartAsync(int cartId, ItemRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _cartService.AddToCartAsync(UserId, cartId, request.Map(), cancellationToken);
        return result switch
        {
            SuccessResult<Cart> successResult => HandleSuccess(successResult.Data?.Map()),
            NotFoundResult<Cart> => HandleNotFound(),
            InvalidResult<Cart> invalidResult => HandleInvalid(invalidResult),
            ErrorResult<Cart> errorResult => HandleErrors(errorResult),
            _ => HandleUnknown()
        };
    }

    [HttpDelete("{cartId}/items/{itemId}")]
    public async Task<IResult> RemoveFromCartAsync(int cartId, int itemId, CancellationToken cancellationToken = default)
    {
        await _cartService.RemoveFromCartAsync(UserId, cartId, itemId, cancellationToken);
        return HandleNoContent();
    }

    [HttpDelete("{cartId}")]
    public async Task<IResult> RemoveCartAsync(int cartId, CancellationToken cancellationToken = default)
    {
        await _cartService.RemoveCartAsync(UserId, cartId, cancellationToken);
        return HandleNoContent();
    }
}