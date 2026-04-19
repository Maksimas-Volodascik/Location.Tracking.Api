using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.DTOs
{
    public class DeviceConfigurationDto
    {
        [Required(ErrorMessage = "Device IMEI is required.")]
        [MaxLength(15, ErrorMessage = "IMEI cannot exceed 15 characters")]
        public string Imei { get; set; } = string.Empty;

        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; } = string.Empty;

        public bool IsEnabled { get; set; } = true;

        [Required (ErrorMessage = "Device model is required.")]
        public string DeviceModelName { get; set; } = string.Empty;

        //todo: network + security configuration
    }
}
