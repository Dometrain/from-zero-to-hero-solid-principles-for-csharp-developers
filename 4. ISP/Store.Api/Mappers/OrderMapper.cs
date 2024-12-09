using Store.Api.Contracts.Responses;
using Store.Application.Models;

namespace Store.Api.Mappers;

public static class OrderMapper
{
    public static PagedResponse<OrderResponse> Map(this Paged<Order> paged) => new PagedResponse<OrderResponse>
    {
        Page = paged.Page,
        TotalPages = paged.TotalPages,
        PageSize = paged.PageSize,
        Data = paged.Data.Select(x => x.Map()).ToList()
    };

    public static OrderResponse Map(this Order order) => new OrderResponse
    {
        OrderId = order.Id,
        OrderDate = order.CreatedAt,
        OrderAmount = order.Total,
        DeliveryCost = order.DeliveryCost,
        Tax = order.Tax,
        Items = order.Items.Select(x => x.Map()).ToList()
    };

    public static IEnumerable<ReportResponse> Map(this IEnumerable<OrderReport> reports) => reports.Select(x => x.Map());

    public static ReportResponse Map(this OrderReport report) => new ReportResponse
    {
        Date = report.Date,
        OrderCount = report.OrderCount,
        OrderTotal = report.OrderTotal
    };
}
