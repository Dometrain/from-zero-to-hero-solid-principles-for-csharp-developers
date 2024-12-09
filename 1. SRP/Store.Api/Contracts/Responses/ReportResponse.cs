namespace Store.Api.Contracts.Responses;

public class ReportResponse
{
    public string Date { get; set; }
    public long OrderCount { get; set; }
    public decimal OrderTotal { get; set; }
}