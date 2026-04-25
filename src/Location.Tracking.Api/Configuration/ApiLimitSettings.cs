using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Shared
{
    public class ApiLimitSettings
    {
        public bool AutoReplenishemnt { get; set; }
        public int Window { get; set; }
        public int PermitLimit { get; set; }
        public int QueueLimit { get; set; }
    }
}
