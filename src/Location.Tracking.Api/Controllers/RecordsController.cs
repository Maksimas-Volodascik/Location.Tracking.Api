using Asp.Versioning;
using Location.Tracking.Application.RawRecords;
using Location.Tracking.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Location.Tracking.Api.Controllers
{
    [Authorize]
    [ApiVersion(1)]
    [Route("v{v:apiVersion}/[controller]")]
    [ApiController]
    public class RecordsController : ControllerBase
    {
        private readonly IRecordService _recordService;
        public RecordsController(IRecordService recordService)
        {
            _recordService = recordService;
        }

        [HttpGet("{deviceId}")]
        public async Task<ActionResult<Device>> GetDeviceRecords(Guid deviceId)
        {
            var response = await _recordService.GetAllRecords(deviceId);

            return Ok(response.Data);
        }
    }
}
