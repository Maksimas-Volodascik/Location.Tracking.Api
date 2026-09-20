using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Dashboard.Dtos
{
    public record UsersMetrics
    {
        public int Total { get; init; } = 0;
        public int Admin { get; init; } = 0;
        public int Users { get; init; } = 0;
    }
}
