using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.DTOs.Users
{
    public record UsersMetrics
    {
        public int Total { get; init; }
        public int Admin { get; init; }
        public int Users { get; init; }
    }
}
