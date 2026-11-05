using Asp.Versioning;
using Location.Tracking.Application.Workspaces;
using Location.Tracking.Application.Workspaces.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Location.Tracking.Api.Controllers
{
    [Authorize]
    [ApiVersion(1, Deprecated = true)]
    [Route("v{v:apiVersion}/[controller]")]
    [ApiController]
    public class WorkspaceController : ControllerBase
    {
        private readonly IWorkspaceService _workspaceService;
        public WorkspaceController(IWorkspaceService workspaceService)
        {
            _workspaceService = workspaceService;
        }

        [HttpGet]
        public async Task<ActionResult<WorkspaceData>> GetWorkspaceData()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException("Missing Name Identifier");

            var response = await _workspaceService.GetUserWorkspaceAsync(new Guid(userId));

            return Ok(response.Data);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateWorkspace(Guid id, UpdateWorkspaceRequest request)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException("Missing Name Identifier");

            var result = await _workspaceService.UpdateWorkspaceAsync(request, new Guid(userId), id);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error.ErrorMessage);
            }

            return Ok();
        }
    }
}
