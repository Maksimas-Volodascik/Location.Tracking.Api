using Location.Tracking.Application.DTOs.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Interfaces.Services
{
    public interface IRecordService
    {
        Task<RecordsMetrics> GetRecordsMetricsAsync();
    }
}
