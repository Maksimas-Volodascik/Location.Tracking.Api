using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Application.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.RawRecords.Query
{
    public class GetDeviceRecordsQueryHandler : IRequestHandler<GetDeviceRecordsQuery, Result<IEnumerable<RecordMessage>>>
    {
        private readonly IRecordRepository _recordRepository;
        public GetDeviceRecordsQueryHandler(IRecordRepository recordRepository)
        {
            _recordRepository = recordRepository;
        }

        public async Task<Result<IEnumerable<RecordMessage>>> Handle(GetDeviceRecordsQuery request, CancellationToken cancellationToken)
        {
            var result = await _recordRepository.GetDeviceRecords(request.DeviceId);

            return Result<IEnumerable<RecordMessage>>.Success(result);
        }
    }
}
