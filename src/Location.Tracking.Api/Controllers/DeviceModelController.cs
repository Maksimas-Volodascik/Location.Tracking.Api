using Asp.Versioning;
using Location.Tracking.Application.DeviceModels.Commands.CreateDeviceModel;
using Location.Tracking.Application.DeviceModels.Commands.DeleteDeviceModel;
using Location.Tracking.Application.DeviceModels.Commands.UpdateDeviceModel;
using Location.Tracking.Application.DeviceModels.Query.GetAllDeviceModels;
using Location.Tracking.Application.DeviceModels.Query.GetDeviceModelById;
using Location.Tracking.Domain.Entities;
using MediatR;
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
        private readonly IMediator _mediator;
        public DeviceModelController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Device>>> GetAllDeviceModelsAsync()
        {
            var deviceModels = await _mediator.Send(new GetAllDeviceModelsQuery());

            return Ok(deviceModels);
        }

        //[Authorize(Roles = "User")]
        [HttpGet("{Id}")]
        public async Task<ActionResult<IEnumerable<Device>>> GetDeviceModelByIdAsync(Guid Id)
        {
            GetDeviceModelByIdQuery query = new GetDeviceModelByIdQuery { DeviceModelId = Id };

            var deviceModels = await _mediator.Send(query);

            return Ok(deviceModels);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateDeviceModelAsync([FromQuery] CreateDeviceModelCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsSuccess == false) return BadRequest(result.Error!.ErrorMessage);

            return Ok();
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPatch("{deviceId}")]
        public async Task<IActionResult> UpdateDeviceModelAsync([FromQuery] UpdateDeviceModelCommand command, Guid deviceId)
        {
            command.DeviceModelId = deviceId;

            await _mediator.Send(command);

            return Ok();
        }

        [Authorize(Roles = "User, Admin")]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteDeviceModelAsync(Guid Id)
        {
            DeleteDeviceModelCommand command = new DeleteDeviceModelCommand { deviceModelId = Id };

            await _mediator.Send(command);
            
            return Ok();
        }
    }
}
