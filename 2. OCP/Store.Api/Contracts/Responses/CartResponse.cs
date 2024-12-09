using System.Linq;

namespace Store.Api.Contracts.Responses;

public class CartResponse
{
    public int CartId { get; set; }
    public IEnumerable<ItemResponse> Items { get; set; } = new List<ItemResponse>();
    public decimal TotalAmount => Items.Sum(x => x.Quantity * x.Product.Price);
}