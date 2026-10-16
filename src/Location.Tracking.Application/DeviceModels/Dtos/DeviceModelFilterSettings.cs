using Location.Tracking.Application.Shared.Paging;

namespace Location.Tracking.Application.DeviceModels.Dtos
{
    public class DeviceModelFilterSettings : PaginationParams
    {
        public string? Name { get; set; }
        public string? ProtocolName { get; set; }
        public string? Description { get; set; } 
    }
}
