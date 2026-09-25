using Location.Tracking.Application.RawRecords.Dtos;
using Location.Tracking.Application.Shared.Interface;
using Location.Tracking.Application.Shared.Results;
using Microsoft.EntityFrameworkCore;

namespace Location.Tracking.Application.RawRecords
{
    public class RecordService : IRecordService
    {
        private readonly ITrackingDbContext _context;
        public RecordService(ITrackingDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<RecordMessage>>> GetAllRecords(Guid deviceId)
        {
            var rawRecordList = await _context.RawRecords
                                .Select(rec => new RecordMessage
                                {
                                    ReceivedAt = rec.ReceivedAt,
                                    ExpiresAt = rec.ExpiresAt,
                                    ParsedData = rec.ParsedData,
                                    RawData = rec.RawData
                                }).ToListAsync();

            return Result<IEnumerable<RecordMessage>>.Success(rawRecordList);
        }
    }
}
