using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Domain.Entities
{
    public class DeviceModel
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; } = string.Empty; //Device name FMC125(teltonika) Plug5(Ruptela) etc..
        
        [MaxLength(25)]
        public string ProtocolName { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Description { get; set; } = string.Empty; //OBD tracker, basic tracker etc..


        public Device Device { get; set; } = null!;
    }
}
