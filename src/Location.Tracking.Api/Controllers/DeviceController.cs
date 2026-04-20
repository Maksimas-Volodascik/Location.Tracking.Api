using Asp.Versioning;
using Location.Tracking.Application.DTOs;
using Location.Tracking.Application.Interfaces.Services;
using Location.Tracking.Application.Shared;
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

        [HttpGet("{deviceId}")]
        public async Task<ActionResult<Device>> GetDeviceByIdAsync(Guid deviceId)
        {
            var device = await _deviceService.GetDeviceByIdAsync(deviceId);

            return Ok(device);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDeviceAsync([FromQuery] DeviceConfigurationDto deviceConfiguration)
        {
            var result = await _deviceService.CreateNewDeviceAsync(deviceConfiguration);

            if (result.IsSuccess == false) return BadRequest(result.Error!.ErrorMessage);

            return Ok();
        }

        [HttpPatch("{deviceId}")]
        public async Task<IActionResult> UpdateDeviceAsync([FromQuery] DeviceConfigurationDto deviceConfiguration, Guid deviceId)
        {
            await _deviceService.UpdateDeviceAsync(deviceConfiguration, deviceId);

            return Ok(deviceId);
        }

        [HttpDelete("{deviceId}")]
        public async Task<IActionResult> DeleteDeviceAsync(Guid deviceId)
        {
            await _deviceService.DeleteDeviceAsync(deviceId);
            return Ok();
        }
    }
}
