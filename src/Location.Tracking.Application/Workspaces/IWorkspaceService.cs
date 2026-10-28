using Location.Tracking.Application.Shared.Results;
using Location.Tracking.Domain.Entities;

namespace Location.Tracking.Application.Workspaces
{
    public interface IWorkspaceService
    {
        public Workspace AddNewWorkspace(string name);
        public Task<Result<Workspace>> GetById(Guid id);
    }
}
