using Store.Application.Models;
using Store.Common.Results;

namespace Store.Application.Services;

public interface IReportService
{
    Task<Result<IEnumerable<OrderReport>>> GetOrderReportAsync(DateTime from, DateTime to, ReportInterval reportInterval, CancellationToken cancellationToken);
}