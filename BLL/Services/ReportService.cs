using BLL.Services.Interfaces;
using DAL;
using DAL.Models;
using Domain.Dto;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class ReportService : IReportService
{
    private readonly ApiContext _apiContext;
    private readonly BackgroundWorkerQueue _backgroundWorkerQueue;

    public ReportService(
        ApiContext apiContext, 
        BackgroundWorkerQueue backgroundWorkerQueue)
    {
        _apiContext = apiContext;
        _backgroundWorkerQueue = backgroundWorkerQueue;
    }

    public async Task<QueryInfo?> GetReportInfoById(Guid id)
    {
        return await _apiContext.Queries
            .Include(x => x.Result)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Guid> CreateUserStatisticsReport(UserStatisticRequest model)
    {
        var queryInfo = new QueryInfo();
        await _apiContext.Queries.AddAsync(queryInfo);
        await _apiContext.SaveChangesAsync();
        
        _backgroundWorkerQueue.QueueBackgroundWorkItem(new QueueItem()
        { 
            QueryId = queryInfo.Id,
            Request = model
        });

        return queryInfo.Id;
    }
}