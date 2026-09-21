using Location.Tracking.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Location.Tracking.Application.Devices.Dtos
{
    public class DeviceDetails
    {
        public Guid Id { get; set; }
        public string Imei { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
        public DateTimeOffset? LastSeen { get; set; } = null;
        public DateTimeOffset DateAdded { get; set; } = DateTimeOffset.UtcNow;
        public Guid DeviceModelId { get; set; }
    }
}
