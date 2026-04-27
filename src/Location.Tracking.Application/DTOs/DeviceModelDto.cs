using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.DTOs
{
    public class DeviceModelDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string ProtocolName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
