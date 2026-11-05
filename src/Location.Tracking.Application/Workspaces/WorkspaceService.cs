using Location.Tracking.Application.Shared.Interface;
using Location.Tracking.Application.Shared.Results;
using Location.Tracking.Application.Workspaces.Dtos;
using Location.Tracking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Location.Tracking.Application.Workspaces
{
    public class WorkspaceService : IWorkspaceService
    {
        private readonly ITrackingDbContext _context;
        public WorkspaceService(ITrackingDbContext context)
        {
            _context = context;
        }

        public Workspace AddNewWorkspace(string name)
        {
            var workspace = new Workspace { Name = name };

            _context.Workspaces.Add(workspace);
            return workspace;
        }

        public async Task<Result<WorkspaceData>> GetUserWorkspaceAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return Result<WorkspaceData>.Failure(Errors.UserErrors.UserNotFound);
            }

            var workspace = await _context.Workspaces
                            .Where(w => w.Id == user.WorkspaceId)
                            .Select(w => new WorkspaceData
                            {
                                Id = w.Id,
                                Name = w.Name,
                                CreatedAt = w.CreatedAt,
                                Description = w.Description,
                                DeviceCount = w.Devices.Count(),
                                UserCount = w.Users.Count(),
                                IsActive = w.IsActive                                
                            })
                            .FirstOrDefaultAsync();

            if (workspace == null)
            {
                return Result<WorkspaceData>.Failure(Errors.WorkspaceErrors.WorkspaceNotFound);
            }

            return Result<WorkspaceData>.Success(workspace);
        }

        public async Task<Result> UpdateWorkspaceAsync(UpdateWorkspaceRequest request, Guid userId, Guid workspaceId)
        {
            if (String.IsNullOrEmpty(request.Name))
            {
                return Result.Failure(Errors.WorkspaceErrors.InvalidWorkspaceName);
            }
            
            var workspace = await _context.Workspaces.FirstOrDefaultAsync(w => w.Id == workspaceId
                                   && w.Users.Any(u => u.Id == userId));

            if (workspace is null) return Result.Failure(Errors.WorkspaceErrors.WorkspaceNotFound);

            workspace.Name = request.Name.Trim();
            workspace.Description = request.Description?.Trim() ?? string.Empty;

            await _context.SaveChangesAsync();

            return Result.Success();
        }
    }
}
