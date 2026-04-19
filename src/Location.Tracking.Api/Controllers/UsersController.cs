using Asp.Versioning;
using Location.Tracking.Application.DTOs;
using Location.Tracking.Application.Interfaces.Services;
using Location.Tracking.Application.Shared;
using Location.Tracking.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Location.Tracking.Api.Controllers
{
    [ApiVersion(1, Deprecated = true)]
    [ApiVersion(2)] //v2 for testing
    [Route("v{v:apiVersion}/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("register")]
        public async Task<IActionResult> RegisterAsync([FromQuery] RegisterDto registerDto)
        {
            var response = await _userService.RegisterAsync(registerDto);

            if (response == null) return BadRequest(new string[] { "User exist" });

            return Ok($"user: {response.FirstName} hashed password: {response.PasswordHash}");
        }

        [HttpGet("login")]
        public async Task<IActionResult> LoginAsync([FromQuery] LoginDto registerDto)
        {
            var response = await _userService.LoginAsync(registerDto);

            if (response == null) return BadRequest(new string[] { "user does not exist" });

            return Ok($"token: {response.accessToken}");
        }
    }
}
