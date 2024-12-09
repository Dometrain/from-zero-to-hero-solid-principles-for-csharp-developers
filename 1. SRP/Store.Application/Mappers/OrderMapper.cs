using Store.Application.Models;
using Store.Infrastructure.Models;

namespace Store.Application.Mappers;

public static class OrderMapper
{
    public static Paged<Order> Map(this PagedRecord<OrderRecord> paged) => new Paged<Order>
    {
        Page = paged.Page,
        TotalPages = paged.TotalPages,
        PageSize = paged.PageSize,
        Data = paged.Data.Select(x => x.Map()).ToList()
    };

    public static Order Map(this OrderRecord order) => new Order
    {
        Id = order.OrderId,
        UserId = order.UserId,
        CreatedAt = order.CreatedAt,
        DeliveryCost = order.DeliveryCost,
        Tax = order.Tax,
        Items = order.Items.Select(x => x.Map()).ToList()
    };

    public static OrderRecord Map(this Order order) => new OrderRecord
    {
        OrderId = order.Id,
        UserId = order.UserId,
        CreatedAt = order.CreatedAt,
        DeliveryCost = order.DeliveryCost,
        Tax = order.Tax,
        OrderTotal = order.Total,
        Items = new JsonList<ItemRecord>(order.Items.Select(x => x.Map()).ToList())
    };

    public static IEnumerable<OrderReport> MapDays(this IEnumerable<OrderReportRecord> report)
    {
        return report
                .Select(o => new OrderReport
                {
                    Date = o.OrderDate.ToString("yyyy-MM-dd"),
                    OrderCount = o.OrderCount,
                    OrderTotal = o.OrderTotal
                });
    }

    public static IEnumerable<OrderReport> MapMonths(this IEnumerable<OrderReportRecord> report)
    {
        return report
                .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
                .Select(g => new OrderReport
                {
                    Date = GetMonthDate(g.Key.Month, g.Key.Year),
                    OrderCount = g.Sum(o => o.OrderCount),
                    OrderTotal = g.Sum(o => o.OrderTotal)
                });
    }

    public static IEnumerable<OrderReport> MapYears(this IEnumerable<OrderReportRecord> report)
    {
        return report
                .GroupBy(o => o.OrderDate.Year)
                .Select(g => new OrderReport
                {
                    Date = g.Key.ToString(),
                    OrderCount = g.Sum(o => o.OrderCount),
                    OrderTotal = g.Sum(o => o.OrderTotal)
                });
    }

    private static string GetMonthDate(int monthNumber, int year)
    {
        var month = monthNumber switch
        {
            1 => "January",
            2 => "February",
            3 => "March",
            4 => "April",
            5 => "May",
            6 => "June",
            7 => "July",
            8 => "August",
            9 => "September",
            10 => "October",
            11 => "November",
            12 => "December",
            _ => throw new ArgumentOutOfRangeException(nameof(monthNumber), monthNumber, "Month should be between 1-12")
        };

        return $"{month} {year}";
    }
}
