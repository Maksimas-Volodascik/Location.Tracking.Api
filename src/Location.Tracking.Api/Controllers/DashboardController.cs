using Asp.Versioning;
using Location.Tracking.Application.Dashboard.Query.GetDashboardMetrics;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Location.Tracking.Api.Controllers
{
    //[Authorize]
    [ApiVersion(1)]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<SystemMetrics>> GetDashboardMetrics()
        {
            var response = await _mediator.Send(new GetDashboardMetricsQuery());

            return Ok(response);
        }
    }
}
