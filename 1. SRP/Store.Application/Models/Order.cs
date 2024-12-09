namespace Store.Application.Models;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public IEnumerable<Item> Items { get; set; } = new List<Item>();
    public decimal Total
    {
        get
        {
            return Items.Sum(x => x.Quantity * x.Product.Price) + DeliveryCost + Tax;
        }
    }
    public decimal DeliveryCost { get; set; } = 0;
    public decimal Tax { get; set; } = 0;
    public DateTime CreatedAt { get; set; }
}