using Location.Tracking.Application.Shared.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Devices.Dtos
{
    public class DeviceFilterSettings: PaginationParams
    {
        public string? Imei { get; set; }
        public string? Name { get; set; }
        public bool? IsEnabled { get; set; }
        public DateTimeOffset? LastSeen { get; set; }
        public DateTimeOffset? DateAdded { get; set; }
        public Guid? DeviceModelId { get; set; }
    }
}
