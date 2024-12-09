namespace Store.Infrastructure.Models;

public class ItemRecord
{
    public int ItemId { get; set; }
    public int Quantity { get; set; }
    public ProductRecord Product { get; set; }
    public DateTime CreatedAt { get; set; }
}