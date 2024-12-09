namespace Store.Api.Contracts.Requests;

public class CartRequest
{
    public List<ItemRequest> Items { get; set; } = new List<ItemRequest>();
}