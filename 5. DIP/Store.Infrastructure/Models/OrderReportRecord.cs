namespace Store.Infrastructure.Models;

public class OrderReportRecord
{
    public DateTime OrderDate { get; set; }
    public long OrderCount { get; set; }
    public decimal OrderTotal { get; set; }
}