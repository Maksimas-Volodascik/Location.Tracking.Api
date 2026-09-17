using Asp.Versioning;
using Location.Tracking.Application.Auth;
using Location.Tracking.Application.Auth.Dtos;
using Location.Tracking.Application.Devices.Commands.UpdateDevice;
using Location.Tracking.Application.Users.Commands.DeleteUser;
using Location.Tracking.Application.Users.Commands.Login;
using Location.Tracking.Application.Users.Commands.Register;
using Location.Tracking.Application.Users.Commands.UpdateUser;
using Location.Tracking.Application.Users.Query.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Location.Tracking.Api.Controllers
{
    //[Authorize]
    [ApiVersion(1, Deprecated = true)]
    [ApiVersion(2)] //v2 for testing
    [Route("v{v:apiVersion}/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAuthService _authService;
        public UsersController(IMediator mediator, IAuthService authService)
        {
            _mediator = mediator;
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var response = await _mediator.Send(new GetUsersQuery());

            //if (!response.IsSuccess) return BadRequest(response.Error.ErrorMessage);

            return Ok(response.Data);
        }

        [HttpPatch("{userId:guid}")]
        public async Task<IActionResult> UpdateDeviceAsync([FromBody] UserConfiguration userConfiguration, Guid userId)
        {
            UpdateUserCommand command = new UpdateUserCommand();
            command.UserId = userId;
            command.UserConfiguration= userConfiguration;

            var result = await _mediator.Send(command);

            if (!result.IsSuccess) return NotFound(result.Error!.ErrorMessage);

            return Ok();
        }

        [HttpDelete("{userId:guid}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var response = await _mediator.Send(new DeleteUserCommand { UserId = userId});
            
            if (!response.IsSuccess) return NotFound(response.Error!.ErrorMessage);
            
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
        {
            var response = await _authService.Register(request);

            if (!response.IsSuccess) return BadRequest(response.Error.ErrorMessage);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            var response = await _authService.Login(request);

            if (!response.IsSuccess) return BadRequest(response.Error.ErrorMessage);

            return Ok($"{response.Data.accessToken}");
        }
    }
}
