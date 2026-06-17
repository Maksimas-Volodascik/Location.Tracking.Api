using Location.Tracking.Application.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.RawRecords.Query
{
    public class GetDeviceRecordsQuery : IRequest<Result<IEnumerable<RecordMessage>>> 
    {
        public Guid DeviceId { get; set; }
    }

    public class RecordMessage
    {
        public string RawData { get; set; } = string.Empty;
        public string ParsedData { get; set; } = string.Empty;
        public DateTimeOffset ReceivedAt { get; set; } 
        public DateTimeOffset ExpiresAt { get; set; }
    }
}
