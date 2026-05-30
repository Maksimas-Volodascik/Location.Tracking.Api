using Asp.Versioning;
using Location.Tracking.Application.DTOs.Dashboard;
using Location.Tracking.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Location.Tracking.Api.Controllers
{
    //[Authorize]
    [ApiVersion(1)]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public async Task<ActionResult<SystemMetrics>> GetDashboardMetrics()
        {
            var response = await _dashboardService.GetSystemMetricsAsync();

            return Ok(response);
        }
    }
}
