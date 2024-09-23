using BLL.Services.Interfaces;
using BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BLL;

public static class DependencyInjection
{
    public static void AddBllServices(this IServiceCollection services)
    {
        services.AddTransient<IReportService, ReportService>();
        services.AddTransient<IReportProcessingService, ReportProcessingService>();
        services.AddSingleton<BackgroundWorkerQueue>();
    }
}
