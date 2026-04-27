using Asp.Versioning;
using Location.Tracking.Application.DTOs;
using Location.Tracking.Application.Interfaces.Services;
using Location.Tracking.Application.Services;
using Location.Tracking.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Location.Tracking.Api.Controllers
{
    [ApiVersion(1)]
    [Route("v{v:apiVersion}/[controller]")]
    [ApiController]
    public class DeviceModelController : ControllerBase
    {
        private readonly IDeviceModelService _deviceModelService;
        public DeviceModelController(IDeviceModelService deviceModelService)
        {
            _deviceModelService = deviceModelService;
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Device>>> GetAllDeviceModelsAsync()
        {
            var deviceModels = await _deviceModelService.GetAllDeviceModelsAsync();

            return Ok(deviceModels);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateDeviceModelAsync([FromQuery] DeviceModelDto deviceModelDto)
        {
            var result = await _deviceModelService.CreateDeviceModelAsync(deviceModelDto);

            if (result.IsSuccess == false) return BadRequest(result.Error!.ErrorMessage);

            return Ok();
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPatch("{deviceId}")]
        public async Task<IActionResult> UpdateDeviceModelAsync([FromQuery] DeviceModelDto deviceModelDto, Guid deviceId)
        {
            await _deviceModelService.UpdateDeviceModelAsync(deviceModelDto, deviceId);

            return Ok();
        }

        [Authorize(Roles = "User, Admin")]
        [HttpDelete("{deviceId}")]
        public async Task<IActionResult> DeleteDeviceModelAsync(Guid deviceId)
        {
            await _deviceModelService.DeleteDeviceModelAsync(deviceId);
            return Ok();
        }
    }
}
