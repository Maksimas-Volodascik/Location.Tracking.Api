namespace Location.Tracking.Application.Workspaces.Dtos
{
    public class WorkspaceData
    {
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; }
        public int UserCount { get; set; }
        public int DeviceCount { get; set; }
    }
}
