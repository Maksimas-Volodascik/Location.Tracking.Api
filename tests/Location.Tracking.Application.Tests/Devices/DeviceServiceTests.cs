using AutoMapper;
using Location.Tracking.Application.Devices;
using Location.Tracking.Application.Devices.Dtos;
using Location.Tracking.Application.Shared.Interface;
using Location.Tracking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System.Security.Principal;

namespace Location.Tracking.Application.Tests.Devices
{
    public class DeviceServiceTests
    {
        private readonly Mock<ITrackingDbContext> _mockDbContext;
        private readonly Mock<IMapper> _mapperMock;
        private readonly DeviceService _deviceService;
        public DeviceServiceTests()
        {
            _mockDbContext = new Mock<ITrackingDbContext>();
            _mapperMock = new Mock<IMapper>();
            _deviceService = new DeviceService(_mockDbContext.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task CreateNewDeviceAsync_ValidData_ReturnsSuccess()
        {
            var userId = Guid.NewGuid();
            var createDevice = new CreateDeviceRequest
            {
                DeviceModelId = Guid.NewGuid().ToString(),
                Imei = "123456789012345"
            };

            var users = new List<User> { new User { Id = userId } };
            var devices = new List<Device> { new Device { Id = Guid.NewGuid(), Imei = "111111111111111" } };
            var deviceModels = new List<DeviceModel> { new DeviceModel{ Id = new Guid(createDevice.DeviceModelId) } };
            Mock<DbSet<User>> mockUserDbSet = users.BuildMockDbSet();
            Mock<DbSet<Device>> mockDeviceDbSet = devices.BuildMockDbSet();
            Mock<DbSet<DeviceModel>> mockDeviceModelDbSet = deviceModels.BuildMockDbSet();

            _mockDbContext.Setup(u => u.Users)
                .Returns(mockUserDbSet.Object);   

            _mockDbContext.Setup(u => u.Devices)
                .Returns(mockDeviceDbSet.Object);   

            _mockDbContext.Setup(u => u.DeviceModel)
                .Returns(mockDeviceModelDbSet.Object);   

            mockUserDbSet.Setup(u => u.FindAsync(It.IsAny<object[]>())).ReturnsAsync(users.FirstOrDefault(u => u.Id == userId));

            mockDeviceModelDbSet.Setup(dm => dm.FindAsync(It.IsAny<object[]>()))
                .ReturnsAsync(deviceModels.FirstOrDefault(dm => dm.Id == new Guid(createDevice.DeviceModelId)));

            _mockDbContext.Setup(d => d.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var result = await _deviceService.CreateNewDeviceAsync(createDevice, userId);

            Assert.True(result.IsSuccess);
        }
    }
}
