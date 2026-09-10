using Location.Tracking.Application.Interfaces.Repositories;
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
    public class GetLogEntriesQueryHandler : IRequestHandler<GetLogEntriesQuery, Result<IEnumerable<LogEntryDto>>>
    {
        private readonly ILogEntryRepository _entryRepository;
        public GetLogEntriesQueryHandler(ILogEntryRepository entryRepository)
        {
            _entryRepository = entryRepository;
        }

        public async Task<Result<IEnumerable<LogEntryDto>>> Handle(GetLogEntriesQuery request, CancellationToken cancellationToken)
        {
            var result = await _entryRepository.GetAllLogEntriesAsync();

            return Result<IEnumerable<LogEntryDto>>.Success(result);
        }
    }
}
