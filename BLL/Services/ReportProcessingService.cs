using BLL.Services.Interfaces;
using DAL;
using DAL.Models;
using Domain.Dto;
using Domain.Options;
using Microsoft.Extensions.Options;

namespace BLL.Services;

public class ReportProcessingService : IReportProcessingService
{
    private readonly ApiContext _apiContext;
    private readonly int _executingTimeDelayInMs;

    public ReportProcessingService(ApiContext apiContext, IOptions<ReportProcessingOption> options)
    {
        _apiContext = apiContext;
        _executingTimeDelayInMs = options.Value?.ExecutingTimeDelayInMs ?? throw new ArgumentException(null, nameof(options));
    }

    public async Task Process(QueueItem item)
    {
        var queryInfo = await _apiContext.Queries.FindAsync(item.QueryId);

        if (queryInfo == null)
            return;

        var step = _executingTimeDelayInMs <= 1000 ? 100 : 1000;
        var start = DateTime.Now;
        var next = start.AddMilliseconds(step);
        var end = start.AddMilliseconds(_executingTimeDelayInMs);

        while (DateTime.Now < end)
        {
            if (DateTime.Now <= next)
                continue;

            queryInfo.Percentage = Math.Round((DateTime.Now - start).TotalMilliseconds / _executingTimeDelayInMs * 100);
            await _apiContext.SaveChangesAsync();

            next = next.AddMilliseconds(step);
        }

        queryInfo.Percentage = 100;
        queryInfo.Result = new QueryResult()
        {
            UserId = item.Request!.UserId,
            CountSignIn = new Random().Next(0, 100)
        };

        await _apiContext.SaveChangesAsync();
    }
}
