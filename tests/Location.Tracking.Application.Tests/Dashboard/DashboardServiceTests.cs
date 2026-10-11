using Location.Tracking.Application.Dashboard;
using Location.Tracking.Application.Shared.Interface;
using Location.Tracking.Application.Tests.Shared;
using Location.Tracking.Domain.Entities;
using Location.Tracking.Domain.Enums;
using Moq;

namespace Location.Tracking.Application.Tests.Dashboard
{
    public class DashboardServiceTests
    {
        private static readonly DateTimeOffset Now = DateTimeOffset.UtcNow;
        private static readonly DateTimeOffset StartOfToday = new(Now.UtcDateTime.Date, TimeSpan.Zero);

        private readonly Mock<ITrackingDbContext> _mockDbContext;
        private readonly DashboardService _dashboardService;

        public DashboardServiceTests()
        {
            _mockDbContext = new Mock<ITrackingDbContext>();
            _mockDbContext.Setup(c => c.Users).Returns(DbSetMockFactory.Create(new List<User>(), x => x.Id).Object);
            _mockDbContext.Setup(c => c.RawRecords).Returns(DbSetMockFactory.Create(new List<RawRecord>(), x => x.Id).Object);
            _mockDbContext.Setup(c => c.Devices).Returns(DbSetMockFactory.Create(new List<Device>(), x => x.Id).Object);

            _dashboardService = new DashboardService(_mockDbContext.Object);
        }

        [Fact]
        public async Task GetDashboardMetricsAsync_EmptyDatabase_ReturnsZeroMetrics()
        {
            var result = await _dashboardService.GetDashboardMetricsAsync();

            Assert.True(result.IsSuccess, result.Error?.ErrorMessage);
            var metrics = result.Data!;
            Assert.Equal(0, metrics.Users.Total);
            Assert.Equal(0, metrics.Users.Users);
            Assert.Equal(0, metrics.Users.Admin);
            Assert.Equal(0, metrics.Records.Total);
            Assert.Equal(0, metrics.Records.Daily);
            Assert.Equal(0, metrics.Devices.Total);
            Assert.Equal(0, metrics.Devices.Weekly);
        }

        [Fact]
        public async Task GetDashboardMetricsAsync_UsersWithDifferentRoles_CountsByRole()
        {
            var users = new List<User>
            {
                new() { Id = Guid.NewGuid(), Role = UserRole.Unknown },
                new() { Id = Guid.NewGuid(), Role = UserRole.Demo },
                new() { Id = Guid.NewGuid(), Role = UserRole.User },
                new() { Id = Guid.NewGuid(), Role = UserRole.User },
                new() { Id = Guid.NewGuid(), Role = UserRole.Admin }
            };
            _mockDbContext.Setup(c => c.Users).Returns(DbSetMockFactory.Create(users, x => x.Id).Object);

            var result = await _dashboardService.GetDashboardMetricsAsync();

            Assert.Equal(5, result.Data!.Users.Total);
            Assert.Equal(2, result.Data.Users.Users);
            Assert.Equal(1, result.Data.Users.Admin);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(720, 1)]
        [InlineData(1439, 1)]
        [InlineData(-1, 0)]
        [InlineData(1440, 0)]
        public async Task GetDashboardMetricsAsync_RecordReceivedAt_CountsDailyWithinCurrentUtcDay(int dateStart, int expectedDaily)
        {
            var records = new List<RawRecord>
            {
                new() { Id = Guid.NewGuid(), ReceivedAt = StartOfToday.AddMinutes(dateStart) }
            };
            _mockDbContext.Setup(c => c.RawRecords).Returns(DbSetMockFactory.Create(records, x => x.Id).Object);

            var result = await _dashboardService.GetDashboardMetricsAsync();

            Assert.Equal(expectedDaily, result.Data!.Records.Daily);
            Assert.Equal(1, result.Data.Records.Total);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(8640, 1)]
        [InlineData(10080, 0)]
        [InlineData(10081, 0)]
        [InlineData(11520, 0)]
        public async Task GetDashboardMetricsAsync_DeviceAddedAt_CountsWeeklyWithinLastSevenDays(int timeInPast, int expectedWeekly)
        {
            var devices = new List<Device>
            {
                new() { Id = Guid.NewGuid(), DateAdded = Now.AddMinutes(-timeInPast) }
            };
            _mockDbContext.Setup(c => c.Devices).Returns(DbSetMockFactory.Create(devices, x => x.Id).Object);

            var result = await _dashboardService.GetDashboardMetricsAsync();

            Assert.Equal(expectedWeekly, result.Data!.Devices.Weekly);
            Assert.Equal(1, result.Data.Devices.Total);
        }

    }
}
