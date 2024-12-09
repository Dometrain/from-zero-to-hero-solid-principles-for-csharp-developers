namespace Store.Infrastructure.Models;

public class CartRecord
{
    public int CartId { get; set; }
    public int UserId { get; set; }
    public JsonList<ItemRecord> Items { get; set; } = new JsonList<ItemRecord>();
    public DateTime CreatedAt { get; set; }
}