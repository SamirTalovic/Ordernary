using Microsoft.AspNetCore.Mvc;
using Ordernary.Models.DTOs;
using Ordernary.Repositories.Implementation;
using Ordernary.Repositories.Interface;
using Ordernary.Services.ServiceInterfaces;

namespace Ordernary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportInterface _reportRepository;
        private readonly IDailyReportService _dailyReportService;
        public ReportController(IReportInterface reportRepository, IDailyReportService dailyReportService)
        {
            _reportRepository = reportRepository;
            _dailyReportService = dailyReportService;
        }

        [HttpGet("dailyreport")]
        public async Task<ActionResult<DailyReportResponseDTO>> GetDailyReport([FromQuery] DateTime date)
        {
            var report = await _reportRepository.GetDailyReportAsync(date);
            return Ok(report);
        }
        [HttpPost("send-daily-report")]
        public async Task<IActionResult> SendDailyReport()
        {
            await _dailyReportService.SendDailyReport();
            return Ok("Daily report sent successfully.");
        }
    }
}
