using AutoMapper;
using Location.Tracking.Application.Devices;
using Location.Tracking.Application.Devices.Dtos;
using Location.Tracking.Application.Shared.Interface;
using Location.Tracking.Application.Shared.Results;
using Location.Tracking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;

namespace Location.Tracking.Application.Tests.Devices
{
    public class DeviceServiceTests
    {
        private readonly Mock<ITrackingDbContext> _mockDbContext;
        private readonly Mock<IMapper> _mapperMock;
        private readonly DeviceService _deviceService;

        private static readonly Guid ExistingUserId = new("11111111-1111-1111-1111-111111111111");
        private static readonly Guid ExistingDeviceModelId = new("22222222-2222-2222-2222-222222222222");
        private const string ExistingDeviceImei= "111111111111111";

        public DeviceServiceTests()
        {
            _mockDbContext = new Mock<ITrackingDbContext>();

            var users = new List<User> { new User { Id = ExistingUserId } };
            var devices = new List<Device> { new Device { Id = Guid.NewGuid(), Imei = ExistingDeviceImei } };
            var deviceModels = new List<DeviceModel> { new DeviceModel { Id = ExistingDeviceModelId } };

            _mockDbContext.Setup(u => u.Users)
                .Returns(MockDbSet(users, u => u.Id).Object);

            _mockDbContext.Setup(u => u.Devices)
                .Returns(MockDbSet(devices, u => u.Id).Object);

            _mockDbContext.Setup(u => u.DeviceModel)
                .Returns(MockDbSet(deviceModels, u => u.Id).Object);

            _mockDbContext.Setup(d => d.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

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
            var createDevice = new CreateDeviceRequest
            {
                DeviceModelId = ExistingDeviceModelId.ToString(),
                Imei = "123456789012345"
            }; 

            var result = await _deviceService.CreateNewDeviceAsync(createDevice, ExistingUserId);

            Assert.True(result.IsSuccess, result.Error?.ErrorMessage);
            _mockDbContext.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateNewDeviceAsync_InvalidUser_ReturnsFailure()
        {
            var userId = Guid.NewGuid();
            var createDevice = new CreateDeviceRequest
            {
                DeviceModelId = ExistingDeviceModelId.ToString(),
                Imei = "123456789012345"
            };

            var result = await _deviceService.CreateNewDeviceAsync(createDevice, userId);

            Assert.Equal(Errors.UserErrors.UserNotFound.ErrorMessage, result.Error?.ErrorMessage);
            _mockDbContext.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CreateNewDeviceAsync_MissingModel_ReturnsFailure()
        {
            var createDevice = new CreateDeviceRequest
            {
                DeviceModelId = Guid.NewGuid().ToString(),
                Imei = "123456789012345"
            };

            var result = await _deviceService.CreateNewDeviceAsync(createDevice, ExistingUserId);

            Assert.Equal(Errors.DeviceModelErrors.DeviceModelNotFound.ErrorMessage, result.Error?.ErrorMessage);
            _mockDbContext.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CreateNewDeviceAsync_DuplicateIMEI_ReturnsFailure()
        {
            var createDevice = new CreateDeviceRequest
            {
                DeviceModelId = ExistingDeviceModelId.ToString(),
                Imei = ExistingDeviceImei
            };

            var result = await _deviceService.CreateNewDeviceAsync(createDevice, ExistingUserId);

            Assert.Equal(Errors.DeviceErrors.DeviceExists.ErrorMessage, result.Error?.ErrorMessage);
            _mockDbContext.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
