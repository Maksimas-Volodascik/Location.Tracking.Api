using Location.Tracking.Application.Shared.Results;
using Location.Tracking.Application.Workspaces.Dtos;
using Location.Tracking.Domain.Entities;

namespace Location.Tracking.Application.Workspaces
{
    public interface IWorkspaceService
    {
        public Workspace AddNewWorkspace(string name);
        public Task<Result<WorkspaceData>> GetUserWorkspaceAsync(Guid userId);
        public Task<Result> UpdateWorkspaceAsync(UpdateWorkspaceRequest request, Guid userId, Guid workspaceId);
    }
}
