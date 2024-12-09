using Store.Application.Models;
using Store.Infrastructure.Models;

namespace Store.Application.Mappers;

public static class ItemMapper
{
    public static ItemRecord Map(this Item item) => new ItemRecord
    {
        ItemId = item.ItemId,
        Product = item.Product.Map(),
        Quantity = item.Quantity
    };

    public static Item Map(this ItemRecord item) => new Item
    {
        ItemId = item.ItemId,
        Product = item.Product.Map(),
        Quantity = item.Quantity
    };
}