using Store.Application.Models;
using Store.Application.Mappers;
using Store.Infrastructure.Data;
using Store.Common.Results;

namespace Store.Application.Services;

public class ReportService
{

    private readonly OrderRepository _orderRepository;

    public ReportService()
    {
        _orderRepository = new OrderRepository();
    }

    public async Task<Result<IEnumerable<OrderReport>>> GetOrderReportAsync(DateTime from, DateTime to, ReportInterval reportInterval, CancellationToken cancellationToken)
    {
        var report = await _orderRepository.GetOrderReportAsync(from, to, cancellationToken);

        return reportInterval switch
        {
            ReportInterval.Day => new SuccessResult<IEnumerable<OrderReport>>(report.MapDays()),
            ReportInterval.Month => new SuccessResult<IEnumerable<OrderReport>>(report.MapMonths()),
            ReportInterval.Year => new SuccessResult<IEnumerable<OrderReport>>(report.MapYears()),
            _ => throw new ArgumentException("Invalid report interval specified"),
        };
    }
}