using Asp.Versioning;
using Location.Tracking.Application.DTOs;
using Location.Tracking.Application.Interfaces.Services;
using Location.Tracking.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Location.Tracking.Api.Controllers
{
    [ApiVersion(1)]
    [Route("v{v:apiVersion}/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _deviceService;
        public DeviceController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Device>>> GetAllDevicesAsync()
        {
            var devices = await _deviceService.GetAllDevicesAsync();

            return Ok(devices);
        }

        [HttpGet("{uuid}")]
        public async Task<ActionResult<Device>> GetDeviceByIdAsync(Guid uuid)
        {
            var device = await _deviceService.GetDeviceByIdAsync(uuid);

            return Ok(device);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDeviceAsync([FromQuery] DeviceConfigurationDto deviceConfiguration)
        {
            await _deviceService.CreateNewDeviceAsync(deviceConfiguration);

            return Ok();
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateDeviceAsync()
        {
            return Ok();
        }

        [HttpDelete("{uuid}")]
        public async Task<IActionResult> DeleteDeviceAsync(Guid uuid)
        {
            await _deviceService.DeleteDeviceAsync(uuid);
            return Ok();
        }
    }
}
