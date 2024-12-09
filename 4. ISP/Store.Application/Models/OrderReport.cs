namespace Store.Application.Models;

public class OrderReport
{
    public string Date { get; set; }
    public long OrderCount { get; set; }
    public decimal OrderTotal { get; set; }
}