using Location.Tracking.Application.RawRecords.Dtos;
using Location.Tracking.Application.Shared.Results;

namespace Location.Tracking.Application.RawRecords
{
    public interface IRecordService
    {
        public Task<Result<IEnumerable<RecordMessage>>> GetAllRecords(Guid deviceId);
    }
}
