using Asp.Versioning;
using Location.Tracking.Application.Devices;
using Location.Tracking.Application.Devices.Dtos;
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
            var response = await _deviceService.GetAllDevicesAsync();

            return Ok(response.Data);
        }

        [HttpGet("{deviceId}")]
        public async Task<ActionResult<Device>> GetDeviceByIdAsync(Guid deviceId)
        {
            var response = await _deviceService.GetDeviceByIdAsync(deviceId);

            if (!response.IsSuccess)
            {
                return BadRequest(response.Error!.ErrorMessage);
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDeviceAsync([FromBody] CreateDeviceRequest createDeviceRequest)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException("Missing Name Identifier");
            

            var response = await _deviceService.CreateNewDeviceAsync(createDeviceRequest, new Guid(userId));

            if (response.IsSuccess == false) return BadRequest(response.Error!.ErrorMessage);

            return Ok();
        }

        [HttpPatch("{deviceId}")]
        public async Task<IActionResult> UpdateDeviceAsync([FromBody] UpdateDeviceRequest updateDeviceRequest, Guid deviceId)
        {
            var response = await _deviceService.UpdateDeviceAsync(deviceId, updateDeviceRequest);

            if (!response.IsSuccess) return NotFound(response.Error!.ErrorMessage);

            return Ok();
        }

        [HttpDelete("{deviceId}")]
        public async Task<IActionResult> DeleteDeviceAsync(Guid deviceId)
        {
            var response = await _deviceService.DeleteDeviceAsync(deviceId);

            if (!response.IsSuccess) return NotFound(response.Error!.ErrorMessage);

            return NoContent();
        }
    }
}
