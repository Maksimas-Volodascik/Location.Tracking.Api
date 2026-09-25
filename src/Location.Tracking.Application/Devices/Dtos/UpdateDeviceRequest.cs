using System.ComponentModel.DataAnnotations;

namespace Location.Tracking.Application.Devices.Dtos
{
    public class UpdateDeviceRequest
    {
        public Guid Id { get; set; }
        [MaxLength(15, ErrorMessage = "IMEI cannot exceed 15 characters")]
        public required string Imei { get; set; }

        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string? Name { get; set; }

        public bool IsEnabled { get; set; } = false;
        public required Guid DeviceModelId { get; set; }
    }
}
