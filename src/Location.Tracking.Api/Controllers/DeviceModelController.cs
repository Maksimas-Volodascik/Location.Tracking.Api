using Asp.Versioning;
using Location.Tracking.Application.DTOs;
using Location.Tracking.Application.Interfaces.Services;
using Location.Tracking.Application.Services;
using Location.Tracking.Domain.Entities;
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


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Device>>> GetAllDeviceModelsAsync()
        {
            var deviceModels = await _deviceModelService.GetAllDeviceModelsAsync();

            return Ok(deviceModels);
        }

        [HttpGet]
        public async Task<ActionResult<Device>> GetDeviceModelByNameAsync(string deviceModelName)
        {
            var deviceModel = await _deviceModelService.GetDeviceModelByNameAsync(deviceModelName);

            return Ok(deviceModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDeviceModelAsync([FromQuery] DeviceModelDto deviceModelDto)
        {
            var result = await _deviceModelService.CreateDeviceModelAsync(deviceModelDto);

            if (result.IsSuccess == false) return BadRequest(result.Error!.ErrorMessage);

            return Ok();
        }

        [HttpPatch("{deviceId}")]
        public async Task<IActionResult> UpdateDeviceModelAsync([FromQuery] DeviceModelDto deviceModelDto, Guid deviceId)
        {
            await _deviceModelService.UpdateDeviceModelAsync(deviceModelDto, deviceId);

            return Ok();
        }

        [HttpDelete("{deviceId}")]
        public async Task<IActionResult> DeleteDeviceModelAsync(Guid deviceId)
        {
            await _deviceModelService.DeleteDeviceModelAsync(deviceId);
            return Ok();
        }
    }
}
