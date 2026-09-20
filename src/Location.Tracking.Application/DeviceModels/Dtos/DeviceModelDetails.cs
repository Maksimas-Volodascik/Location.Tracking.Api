using Location.Tracking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.DeviceModels.Dtos
{
    public class DeviceModelDetails
    {
        public Guid Id { get; set; }

        [MaxLength(50)]
        public required string Name { get; set; } //Device name FMC125(teltonika) Plug5(Ruptela) etc..

        [MaxLength(25)]
        public required string ProtocolName { get; set; } //FMC125, Plug5, etc..

        [MaxLength(200)]
        public string Description { get; set; } = string.Empty; //OBD tracker, basic tracker etc..
    }
}
