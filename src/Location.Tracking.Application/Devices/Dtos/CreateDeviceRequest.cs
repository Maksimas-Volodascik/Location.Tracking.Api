using System.ComponentModel.DataAnnotations;

namespace Location.Tracking.Application.Devices.Dtos
{
    public class CreateDeviceRequest
    {
        [MaxLength(15, ErrorMessage = "IMEI cannot exceed 15 characters")]
        public required string Imei { get; set; }

        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; } = string.Empty;

        public bool IsEnabled { get; set; } = false;
        public required Guid DeviceModelId { get; set; }
    }
}
