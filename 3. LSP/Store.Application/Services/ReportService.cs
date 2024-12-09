using Store.Application.Models;
using Store.Application.Mappers;
using Store.Infrastructure.Data;
using Store.Common.Results;
using Store.Common.Helpers;

namespace Store.Application.Services;

public class ReportService : IReportService
{

    private readonly IReadOrders _readOrders;

    public ReportService(IReadOrders readOrders)
    {
        _readOrders = readOrders.NotNull();
    }

    public async Task<Result<IEnumerable<OrderReport>>> GetOrderReportAsync(DateTime from, DateTime to, ReportInterval reportInterval, CancellationToken cancellationToken)
    {
        var report = await _readOrders.GetOrderReportAsync(from, to, cancellationToken);

        return reportInterval switch
        {
            ReportInterval.Day => new SuccessResult<IEnumerable<OrderReport>>(report.MapDays()),
            ReportInterval.Month => new SuccessResult<IEnumerable<OrderReport>>(report.MapMonths()),
            ReportInterval.Year => new SuccessResult<IEnumerable<OrderReport>>(report.MapYears()),
            _ => throw new ArgumentException("Invalid report interval specified"),
        };
    }
}