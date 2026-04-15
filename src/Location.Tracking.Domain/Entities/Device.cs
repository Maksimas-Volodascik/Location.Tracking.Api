using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Domain.Entities
{
    public class Device
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(15)]
        public string Imei { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Name { get; set; } = string.Empty; //User defined name e.g. "Device for VIN xyz123"
        public bool IsEnabled { get; set; } = true;
        public DateTimeOffset? LastSeen { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public DeviceModel DeviceModel { get; set; } = null!;

        public List<RawRecord> Records = new List<RawRecord>();
    }
}
