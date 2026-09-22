namespace Location.Tracking.Application.RawRecords.Dtos
{
    public class RecordMessage
    {
        public string RawData { get; set; } = string.Empty;
        public string ParsedData { get; set; } = string.Empty;
        public DateTimeOffset ReceivedAt { get; set; }
        public DateTimeOffset ExpiresAt { get; set; }
    }
}
