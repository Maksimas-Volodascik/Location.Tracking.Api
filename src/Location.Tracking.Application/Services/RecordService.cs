using Location.Tracking.Application.DTOs.Records;
using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Services
{
    public class RecordService : IRecordService
    {
        private readonly IRecordRepository _recordRepository;
        public RecordService(IRecordRepository recordRepository)
        {
            _recordRepository = recordRepository;
        }

        public async Task<RecordsMetrics> GetRecordsMetricsAsync()
        {
            RecordsMetrics usersMetrics = await _recordRepository.GetRecordsMetrics();

            return usersMetrics;
        }

    }
}
