namespace Location.Tracking.Application.Workspaces.Dtos
{
    public class WorkspaceResponse
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
