using Asp.Versioning;
using Location.Tracking.Application.RawRecords.Query;
using Location.Tracking.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Location.Tracking.Api.Controllers
{
    [Authorize]
    [ApiVersion(1)]
    [Route("v{v:apiVersion}/[controller]")]
    [ApiController]
    public class RecordsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RecordsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{deviceId}")]
        public async Task<ActionResult<Device>> GetDeviceRecords(Guid deviceId)
        {
            var result = await _mediator.Send(new GetDeviceRecordsQuery { DeviceId = deviceId });

            return Ok(result.Data);
        }
    }
}
