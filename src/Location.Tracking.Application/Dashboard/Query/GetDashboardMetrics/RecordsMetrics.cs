using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Dashboard.Query.GetDashboardMetrics
{
    public record RecordsMetrics
    {
        public int Total { get; init; } = 0;
        public int Daily { get; init; } = 0;
    }
}
