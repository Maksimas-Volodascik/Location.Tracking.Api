using Location.Tracking.Application.LogEntries.Dtos;
using Location.Tracking.Application.Shared.Interface;
using Location.Tracking.Application.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace Location.Tracking.Application.LogEntries
{
    public class LogEntriesService : ILogEntriesService
    {
        private readonly ITrackingDbContext _context;
        public LogEntriesService(ITrackingDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<LogEntryDetails>>> GetLogEntriesAsync()
        {
            var logEntries = await _context.LogEntry
                                    .Select(log => new LogEntryDetails
                                    {
                                        Imei = log.Imei,
                                        Message = log.Message,
                                        ReceivedDate = log.ReceivedDate,
                                        Severity = log.Severity,
                                        TraceId = log.TraceId
                                    }).ToListAsync();

            return Result<IEnumerable<LogEntryDetails>>.Success(logEntries);
        }
    }
}
