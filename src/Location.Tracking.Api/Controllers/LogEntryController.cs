using Asp.Versioning;
using Location.Tracking.Application.LogEntries;
using Location.Tracking.Application.LogEntries.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Location.Tracking.Api.Controllers
{
    [ApiVersion(1)]
    [Route("v{v:apiVersion}/[controller]")]
    [ApiController]
    public class LogEntryController : ControllerBase
    {
        private readonly ILogEntriesService _logEntriesService;
        public LogEntryController(ILogEntriesService logEntriesService)
        {
            _logEntriesService = logEntriesService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LogEntryDetails>>> GetAllLogEntries()
        {
            var response = await _logEntriesService.GetLogEntriesAsync();

            return Ok(response.Data);            
        }

    }
}
