namespace Location.Tracking.Application.RawRecords.Dtos
{
    public class DevicePosition
    {
        public Guid Id { get; set; }
        public DateTimeOffset GpsTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        public int Angle { get; set; }
        public DateTimeOffset ReceivedAt { get; set; }
    }
}
