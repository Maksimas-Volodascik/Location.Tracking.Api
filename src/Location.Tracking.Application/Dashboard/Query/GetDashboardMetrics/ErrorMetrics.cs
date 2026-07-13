using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Dashboard.Query.GetDashboardMetrics
{
    public record ErrorMetrics
    {
        public int Total { get; init; } = 0;
        public int Weekly { get; init; } = 0;
    }
}
