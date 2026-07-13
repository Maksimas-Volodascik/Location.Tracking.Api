using Asp.Versioning;
using Location.Tracking.Application.Devices.Commands.CreateNewDevice;
using Location.Tracking.Application.Devices.Commands.DeleteDevice;
using Location.Tracking.Application.Devices.Commands.UpdateDevice;
using Location.Tracking.Application.Devices.Query.GetAllDevices;
using Location.Tracking.Application.Devices.Query.GetDeviceById;
using Location.Tracking.Application.RawRecords.Query;
using Location.Tracking.Application.Shared;
using Location.Tracking.Domain.Entities;
using MediatR;
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
        private readonly IMediator _mediator;
        public DeviceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Device>>> GetAllDevicesAsync()
        {
            var devices = await _mediator.Send(new GetAllDevicesQuery());

            return Ok(devices.Data);
        }

        [HttpGet("{deviceId}")]
        public async Task<ActionResult<Device>> GetDeviceByIdAsync(Guid deviceId)
        {
            var device = await _mediator.Send(new GetDeviceByIdQuery { DeviceId = deviceId});

            return Ok(device);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDeviceAsync([FromBody] CreateNewDeviceRequest deviceConfiguration)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException("Missing Name Identifier");
            var command = new CreateNewDeviceCommand
            {
                DeviceData = deviceConfiguration,
                UserId = Guid.Parse(userId)
            };

            var result = await _mediator.Send(command);

            if (result.IsSuccess == false) return BadRequest(result.Error!.ErrorMessage);

            return Ok();
        }

        [HttpPatch("{deviceId}")]
        public async Task<IActionResult> UpdateDeviceAsync([FromBody] DeviceConfiguration deviceConfiguration, Guid deviceId)
        {
            UpdateDeviceCommand command = new UpdateDeviceCommand();
            command.DeviceId = deviceId;
            command.DeviceConfiguration = deviceConfiguration;

            var result = await _mediator.Send(command);

            if (!result.IsSuccess) return NotFound(result.Error!.ErrorMessage);

            return Ok();
        }

        [HttpDelete("{deviceId}")]
        public async Task<IActionResult> DeleteDeviceAsync(Guid deviceId)
        {
            var result = await _mediator.Send(new DeleteDeviceCommand { DeviceId = deviceId});

            if (!result.IsSuccess) return NotFound(result.Error!.ErrorMessage);

            return Ok();
        }
    }
}
