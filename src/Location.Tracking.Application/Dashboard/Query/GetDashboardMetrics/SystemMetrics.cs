using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Dashboard.Query.GetDashboardMetrics
{
    public record SystemMetrics
    {
        public UsersMetrics Users { get; set; }
        public RecordsMetrics Records { get; set; }
        public DevicesMetrics Devices { get; set; }
    }
}
