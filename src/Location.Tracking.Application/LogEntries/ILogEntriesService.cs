using Location.Tracking.Application.LogEntries.Dtos;
using Location.Tracking.Application.Shared.Results;

namespace Location.Tracking.Application.LogEntries
{
    public interface ILogEntriesService
    {
        public Task<Result<IEnumerable<LogEntryDetails>>> GetLogEntriesAsync();
    }
}
