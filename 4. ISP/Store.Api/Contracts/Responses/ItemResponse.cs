namespace Store.Api.Contracts.Responses;

public class ItemResponse
{
    public int ItemId { get; set; }
    public int Quantity { get; set; }
    public ProductResponse Product { get; set; }
}