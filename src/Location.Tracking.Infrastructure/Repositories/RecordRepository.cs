using Location.Tracking.Application.Dashboard.Query.GetDashboardMetrics;
using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Application.RawRecords.Query;
using Location.Tracking.Domain.Entities;
using Location.Tracking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Infrastructure.Repositories
{
    public class RecordRepository : BaseRepository<RawRecord>, IRecordRepository
    {
        private readonly TrackingDbContext _context;
        public RecordRepository(TrackingDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<RecordsMetrics> GetRecordsMetrics()
        {
            var today = DateTimeOffset.Now.Date;
            var tomorrow = today.AddDays(1);

            var query = await _context.RawRecords
                .Select(r => r.ReceivedAt)
                .GroupBy(_ => 1)
                .Select(r => new RecordsMetrics
                {
                    Daily = r.Count(r => r.Date >= today && r.Date < tomorrow),
                    Total = r.Count()
                }).FirstOrDefaultAsync();

            return query;
        }

        public async Task<IEnumerable<RecordMessage>> GetDeviceRecords(Guid id)
        {
            var query = from R in _context.RawRecords
                        select new RecordMessage
                        {
                            ExpiresAt = R.ExpiresAt,
                            ReceivedAt = R.ReceivedAt,
                            RawData = R.RawData,
                            ParsedData = R.ParsedData,
                        };

            return await query.ToListAsync();
        }
    }
}
