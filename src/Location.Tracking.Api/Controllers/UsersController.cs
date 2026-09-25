using Asp.Versioning;
using Location.Tracking.Application.Auth;
using Location.Tracking.Application.Auth.Dtos;
using Location.Tracking.Application.Users;
using Location.Tracking.Application.Users.Dtos;
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
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        public UsersController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var response = await _userService.GetAllUsers();

            //if (!response.IsSuccess) return BadRequest(response.Error.ErrorMessage);

            return Ok(response.Data);
        }

        [HttpPatch("{userId:guid}")]
        public async Task<IActionResult> UpdateDeviceAsync([FromBody] UserConfiguration userConfiguration, Guid userId)
        {
            var result = await _userService.UpdateUser(userId, userConfiguration);

            if (!result.IsSuccess) return NotFound(result.Error!.ErrorMessage);

            return Ok();
        }

        [HttpDelete("{userId:guid}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var response = await _userService.DeleteUser(userId);
            
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
