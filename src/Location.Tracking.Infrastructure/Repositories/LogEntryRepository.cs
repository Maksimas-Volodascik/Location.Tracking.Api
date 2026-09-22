using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Application.LogEntries.Dtos;
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
    public class LogEntryRepository: BaseRepository<LogEntry>, ILogEntryRepository
    {
        private readonly TrackingDbContext _context;
        public LogEntryRepository(TrackingDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LogEntryDetails>> GetAllLogEntriesAsync()
        {
            var query = from logEntries in _context.LogEntry
                        select new LogEntryDetails{
                            Imei = logEntries.Imei,
                            Message = logEntries.Message,
                            ReceivedDate = logEntries.ReceivedDate,
                            Severity = logEntries.Severity,
                            TraceId = logEntries.TraceId
                        };

            return await query.ToListAsync();
        }
    }
}
