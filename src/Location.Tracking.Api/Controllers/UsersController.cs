using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Location.Tracking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public UsersController()
        {
            
        }

        [HttpGet("/users")]
        public async Task<IActionResult> GetUsers()
        {

            return Ok("Hello");
        }
    }
}
