namespace Location.Tracking.Application.Workspaces.Dtos
{
    public class UpdateWorkspaceRequest
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
