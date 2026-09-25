using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Dashboard.Dtos
{
    public record SystemMetrics
    {
        public UsersMetrics Users { get; set; } = null!;
        public RecordsMetrics Records { get; set; } = null!;
        public DevicesMetrics Devices { get; set; } = null!;
        public ErrorMetrics Errors { get; set; } = null!;
    }
}
