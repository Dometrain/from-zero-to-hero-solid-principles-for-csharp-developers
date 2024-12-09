namespace Store.Application.Models;

public class Cart
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public IEnumerable<Item> Items { get; set; } = new List<Item>();
    public decimal Total
    {
        get
        {
            return Items.Sum(x => x.Quantity * x.Product.Price);
        }
    }
    public DateTime CreatedAt { get; set; }
}