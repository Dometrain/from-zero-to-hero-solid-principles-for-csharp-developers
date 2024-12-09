using Store.Api.Contracts.Requests;
using Store.Api.Contracts.Responses;
using Store.Application.Models;

namespace Store.Api.Mappers;

public static class CartMapper
{
    public static CartResponse Map(this Cart cart) => new CartResponse
    {
        CartId = cart.Id,
        Items = cart.Items.Select(x => x.Map())
    };

    public static Cart Map(this CartRequest cart) => new Cart
    {
        Items = cart.Items.Select(x => x.Map())
    };
}