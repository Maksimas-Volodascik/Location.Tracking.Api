using Asp.Versioning;
using Location.Tracking.Application.DeviceModels;
using Location.Tracking.Application.DeviceModels.Dtos;
using Location.Tracking.Domain.Entities;
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

        //[Authorize(Roles = "User")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Device>>> GetAllDeviceModelsAsync()
        {
            var response = await _deviceModelService.GetAllDeviceModelsAsync();

            return Ok(response.Data);
        }

        //[Authorize(Roles = "User")]
        [HttpGet("{deviceModelId}")]
        public async Task<ActionResult<IEnumerable<Device>>> GetDeviceModelByIdAsync(Guid deviceModelId)
        {
            var response = await _deviceModelService.GetDeviceModelByIdAsync(deviceModelId);

            if (!response.IsSuccess)
            {
                return BadRequest(response.Error!.ErrorMessage);
            }

            return Ok(response.Data);
        }

        //[Authorize(Roles = "User, Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateDeviceModelAsync([FromBody] CreateDeviceModelRequest request)
        {
            var response = await _deviceModelService.CreateNewDeviceModelAsync(request);

            if (!response.IsSuccess) return BadRequest(response.Error!.ErrorMessage);

            return NoContent();
        }

       //[Authorize(Roles = "User, Admin")]
        [HttpPatch("{deviceModelId}")]
        public async Task<IActionResult> UpdateDeviceModelAsync([FromBody] UpdateDeviceModelRequest request, Guid deviceModelId)
        {
            var response = await _deviceModelService.UpdateDeviceModelAsync(deviceModelId, request);

            if (!response.IsSuccess)
            {
                return BadRequest(response.Error!.ErrorMessage);
            }

            return NoContent();
        }

        //[Authorize(Roles = "User, Admin")]
        [HttpDelete("{deviceModelId}")]
        public async Task<IActionResult> DeleteDeviceModelAsync(Guid deviceModelId)
        {
            var response = await _deviceModelService.DeleteDeviceModelAsync(deviceModelId);

            if (!response.IsSuccess)
            {
                return BadRequest(response.Error!.ErrorMessage);
            }

            return NoContent();
        }
    }
}
