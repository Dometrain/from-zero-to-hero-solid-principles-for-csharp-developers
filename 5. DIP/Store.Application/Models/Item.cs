namespace Store.Application.Models;

public class Item
{
    public int ItemId { get; set; }
    public int Quantity { get; set; }
    public Product Product { get; set; }
    public DateTime CreatedAt { get; set; }
}