using Location.Tracking.Application.DTOs;
using Location.Tracking.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

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
        public async Task<IActionResult> Register([FromQuery] LoginDto credentials)
        {
            if (credentials == null) return BadRequest();

            var response = await _userService.RegisterAsync(credentials);

            if (response == null) return BadRequest(new string[] { "User not found", "Invalid ID" });

            return Ok($"{credentials.Email} and {credentials.Password}");
        }
    }
}
