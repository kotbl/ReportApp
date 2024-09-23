using DAL.Models;
using Domain.Dto;

namespace BLL.Services.Interfaces;

public interface IReportService
{
    Task<QueryInfo?> GetReportInfoById(Guid id);
    Task<Guid> CreateUserStatisticsReport(UserStatisticRequest mode);
}
