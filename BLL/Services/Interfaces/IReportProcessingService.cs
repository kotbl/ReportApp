using Domain.Dto;

namespace BLL.Services.Interfaces;

public interface IReportProcessingService
{
    Task Process(QueueItem item);
}
