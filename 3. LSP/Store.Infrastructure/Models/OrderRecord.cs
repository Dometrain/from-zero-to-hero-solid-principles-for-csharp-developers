namespace Store.Infrastructure.Models;

public class OrderRecord
{
    public int OrderId { get; set; }
    public int UserId { get; set; }
    public JsonList<ItemRecord> Items { get; set; } = new JsonList<ItemRecord>();
    public decimal OrderTotal { get; set; }
    public decimal DeliveryCost { get; set; }
    public decimal Tax { get; set; }
    public DateTime CreatedAt { get; set; }
}