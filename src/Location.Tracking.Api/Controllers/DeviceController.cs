using Asp.Versioning;
using Location.Tracking.Application.DTOs;
using Location.Tracking.Application.Interfaces.Services;
using Location.Tracking.Application.Shared;
using Location.Tracking.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Location.Tracking.Api.Controllers
{
    [Authorize]
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
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException("Missing Name Identifier");

            var result = await _deviceService.CreateNewDeviceAsync(deviceConfiguration, userId);

            if (result.IsSuccess == false) return BadRequest(result.Error!.ErrorMessage);

            return Ok(userId);
        }

        [HttpPatch("{deviceId}")]
        public async Task<IActionResult> UpdateDeviceAsync([FromQuery] DeviceConfigurationDto deviceConfiguration, Guid deviceId)
        {
            var result = await _deviceService.UpdateDeviceAsync(deviceConfiguration, deviceId);

            if (!result.IsSuccess) return NotFound(result.Error!.ErrorMessage);

            return Ok();
        }

        [HttpDelete("{deviceId}")]
        public async Task<IActionResult> DeleteDeviceAsync(Guid deviceId)
        {
            var result = await _deviceService.DeleteDeviceAsync(deviceId);

            if (!result.IsSuccess) return NotFound(result.Error!.ErrorMessage);

            return Ok();
        }
    }
}
