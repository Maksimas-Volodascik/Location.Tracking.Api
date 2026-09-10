using Asp.Versioning;
using Location.Tracking.Application.LogEntries.Query;
using Location.Tracking.Application.RawRecords.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Location.Tracking.Api.Controllers
{
    [ApiVersion(1)]
    [Route("v{v:apiVersion}/[controller]")]
    [ApiController]
    public class LogEntryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LogEntryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LogEntryDto>>> GetAllLogEntries()
        {
            var response = await _mediator.Send(new GetLogEntriesQuery());

            return Ok(response);            
        }

    }
}
