using Location.Tracking.Application.DTOs;
using Location.Tracking.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Location.Tracking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("register")]
        public async Task<IActionResult> GetUsers([FromQuery] RegisterDto registerDto)
        {
            var response = await _userService.RegisterAsync(registerDto);

            if (response == null) return BadRequest(new string[] { "User exist" });

            return Ok($"user: {response.FirstName} hashed password: {response.PasswordHash}");
        }
    }
}
