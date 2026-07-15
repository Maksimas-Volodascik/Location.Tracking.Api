using Asp.Versioning;
using Location.Tracking.Application.Users.Commands.Login;
using Location.Tracking.Application.Users.Commands.Register;
using Location.Tracking.Application.Users.Query.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Location.Tracking.Api.Controllers
{
    [ApiVersion(1, Deprecated = true)]
    [ApiVersion(2)] //v2 for testing
    [Route("v{v:apiVersion}/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var response = await _mediator.Send(new GetUsersQuery());

            //if (!response.IsSuccess) return BadRequest(response.Error.ErrorMessage);

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromQuery] RegisterCommand command)
        {
            var response = await _mediator.Send(command);

            if (!response.IsSuccess) return BadRequest(response.Error.ErrorMessage);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromQuery] LoginCommand command)
        {
            var response = await _mediator.Send(command);

            if (!response.IsSuccess) return BadRequest(response.Error.ErrorMessage);

            return Ok($"token: {response.Data.accessToken}");
        }
    }
}
