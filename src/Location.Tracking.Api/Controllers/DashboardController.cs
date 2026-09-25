using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Location.Tracking.Application.Dashboard.Dtos;
using Location.Tracking.Application.Dashboard;
namespace Location.Tracking.Api.Controllers
{
    [Authorize]
    [ApiVersion(1)]
    [Route("v{v:apiVersion}/[controller]")]
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
            var response = await _dashboardService.GetDashboardMetricsAsync();

            return Ok(response);
        }
    }
}
