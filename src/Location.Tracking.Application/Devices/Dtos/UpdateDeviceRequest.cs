using System.ComponentModel.DataAnnotations;

namespace Location.Tracking.Application.Devices.Dtos
{
    public class UpdateDeviceRequest
    {
        public required string Imei { get; set; }

        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string? Name { get; set; }

        public bool IsEnabled { get; set; } = false;
        public required Guid DeviceModelId { get; set; }
    }
}
