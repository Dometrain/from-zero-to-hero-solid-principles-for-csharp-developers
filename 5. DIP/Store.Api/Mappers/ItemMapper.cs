using Store.Api.Contracts.Requests;
using Store.Api.Contracts.Responses;
using Store.Application.Models;

namespace Store.Api.Mappers;

public static class ItemMapper
{
    public static ItemResponse Map(this Item item) => new ItemResponse
    {
        ItemId = item.ItemId,
        Product = item.Product.Map(),
        Quantity = item.Quantity
    };

    public static Item Map(this ItemRequest item) => new Item
    {
        Product = new Product { ProductId = item.ProductId },
        Quantity = item.Quantity
    };
}