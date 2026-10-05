using AutoMapper;
using Location.Tracking.Application.Devices;
using Location.Tracking.Application.Devices.Dtos;
using Location.Tracking.Application.Shared.Interface;
using Location.Tracking.Application.Shared.Results;
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

        private static Mock<DbSet<T>> MockDbSet<T>(List<T> entityList, Func<T, Guid> getId) where T: class
        {
            Mock<DbSet<T>> set = entityList.BuildMockDbSet();
            set.Setup(s => s.FindAsync(It.IsAny<object[]>())).ReturnsAsync((object[] keys) => entityList.FirstOrDefault(x => getId(x) == (Guid)keys[0]));
            return set;
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
            var deviceModels = new List<DeviceModel> { new DeviceModel { Id = new Guid(createDevice.DeviceModelId) } };

            _mockDbContext.Setup(u => u.Users)
                .Returns(MockDbSet(users, u => u.Id).Object);

            _mockDbContext.Setup(u => u.Devices)
                .Returns(MockDbSet(devices, u => u.Id).Object);

            _mockDbContext.Setup(u => u.DeviceModel)
                .Returns(MockDbSet(deviceModels, u => u.Id).Object);

            _mockDbContext.Setup(d => d.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var result = await _deviceService.CreateNewDeviceAsync(createDevice, userId);

            Assert.True(result.IsSuccess, result.Error?.ErrorMessage);
            _mockDbContext.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateNewDeviceAsync_InvalidUser_ReturnsFailure()
        {
            var userId = Guid.NewGuid();
            var createDevice = new CreateDeviceRequest
            {
                DeviceModelId = Guid.NewGuid().ToString(),
                Imei = "123456789012345"
            };

            var users = new List<User> { new User { Id = Guid.NewGuid() } };
            var devices = new List<Device> { new Device { Id = Guid.NewGuid(), Imei = "111111111111111" } };
            var deviceModels = new List<DeviceModel> { new DeviceModel { Id = new Guid(createDevice.DeviceModelId) } };

            _mockDbContext.Setup(u => u.Users)
                .Returns(MockDbSet(users, u => u.Id).Object);

            _mockDbContext.Setup(u => u.Devices)
                .Returns(MockDbSet(devices, u => u.Id).Object);

            _mockDbContext.Setup(u => u.DeviceModel)
                .Returns(MockDbSet(deviceModels, u => u.Id).Object);

            var result = await _deviceService.CreateNewDeviceAsync(createDevice, userId);

            Assert.Equal(result.Error?.ErrorMessage, Errors.UserErrors.UserNotFound.ErrorMessage);
            _mockDbContext.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
