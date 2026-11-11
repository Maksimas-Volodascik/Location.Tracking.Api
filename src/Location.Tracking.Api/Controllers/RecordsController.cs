using Asp.Versioning;
using Location.Tracking.Application.RawRecords;
using Location.Tracking.Application.RawRecords.Dtos;
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

        [HttpGet("{deviceId}/records")]
        public async Task<ActionResult<IEnumerable<RecordMessage>>> GetDeviceRecords(Guid deviceId)
        {
            var response = await _recordService.GetAllRecords(deviceId);

            return Ok(response.Data);
        }

        [HttpGet("{deviceId}/positions")]
        public async Task<ActionResult<IEnumerable<DevicePosition>>> GetDeviceLocations(Guid deviceId)
        {
            var response = await _recordService.GetDeviceLocations(deviceId);

            return Ok(response.Data);
        }
    }
}
