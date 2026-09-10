using Location.Tracking.Application.RawRecords.Query;
using Location.Tracking.Application.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.LogEntries.Query
{
    public class GetLogEntriesQuery : IRequest<Result<IEnumerable<LogEntryDto>>>{ }
}
