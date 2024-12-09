using System.Linq;

namespace Store.Api.Contracts.Responses;

public class OrderResponse
{
    public int OrderId { get; set; }
    public IEnumerable<ItemResponse> Items { get; set; } = new List<ItemResponse>();
    public decimal DeliveryCost { get; set; }
    public decimal Tax { get; set; }
    public decimal OrderAmount { get; set; }
    public DateTime OrderDate { get; set; }
}