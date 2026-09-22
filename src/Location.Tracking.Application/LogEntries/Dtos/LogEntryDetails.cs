using System.ComponentModel.DataAnnotations;


namespace Location.Tracking.Application.LogEntries.Dtos
{
    public class LogEntryDetails
    {
        public Guid TraceId { get; set; }

        [MaxLength(20)]
        public string Imei { get; set; } = string.Empty;
        public DateTimeOffset ReceivedDate { get; set; }

        [MaxLength(2000)]
        public string Message { get; set; } = string.Empty;
        [MaxLength(16)]
        public string Severity { get; set; } = string.Empty;
    }
}
