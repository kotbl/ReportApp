using BLL.Services.Interfaces;
using Domain.Dto;
using Microsoft.AspNetCore.Mvc;

namespace ReportApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        [Route("info")]
        public async Task<IActionResult> Info(Guid id) => 
            Ok(await _reportService.GetReportInfoById(id));

        [HttpPost]
        [Route("user_statistics")]
        public async Task<IActionResult> UserStatistics(UserStatisticRequest model) =>
            Ok(await _reportService.CreateUserStatisticsReport(model));
    }
}
