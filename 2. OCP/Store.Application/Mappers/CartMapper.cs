using Store.Application.Models;
using Store.Infrastructure.Models;

namespace Store.Application.Mappers;

public static class CartMapper
{
    public static Cart Map(this CartRecord cart) => new Cart
    {
        Id = cart.CartId,
        Items = cart.Items.Select(x => x.Map())
    };

    public static CartRecord Map(this Cart cart) => new CartRecord
    {
        Items = new JsonList<ItemRecord>(cart.Items.Select(x => x.Map()))
    };
}