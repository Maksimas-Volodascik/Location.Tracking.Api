using Location.Tracking.Application.RawRecords.Query;
using Location.Tracking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Interfaces.Repositories
{
    public interface ILogEntryRepository: IBaseRepository<LogEntry>
    {
        Task<IEnumerable<LogEntryDto>> GetAllLogEntriesAsync();
    }
}
