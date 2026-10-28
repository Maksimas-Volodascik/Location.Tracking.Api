using Location.Tracking.Application.Shared.Interface;
using Location.Tracking.Application.Shared.Results;
using Location.Tracking.Domain.Entities;
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
            var workspace = new Workspace{ Name = name };

            _context.Workspaces.Add(workspace);
            return workspace;
        }

        public async Task<Result<Workspace>> GetById(Guid id)
        {
            var workspace = await _context.Workspaces.FindAsync(id);

            return workspace is null 
                ? Result<Workspace>.Failure(Errors.WorkspaceErrors.WorkspaceNotFound)
                : Result<Workspace>.Success(workspace);
        }
    }
}
