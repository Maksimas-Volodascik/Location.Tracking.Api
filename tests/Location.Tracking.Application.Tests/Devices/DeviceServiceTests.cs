using AutoMapper;
using Location.Tracking.Application.AutoMapper;
using Location.Tracking.Application.Devices;
using Location.Tracking.Application.Devices.Dtos;
using Location.Tracking.Application.Shared.Interface;
using Location.Tracking.Application.Shared.Results;
using Location.Tracking.Application.Tests.Shared;
using Location.Tracking.Domain.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Location.Tracking.Application.Tests.Devices
{
    public class DeviceServiceTests
    {
        private readonly Mock<ITrackingDbContext> _mockDbContext;
        private static IMapper _mapper;
        private readonly DeviceService _deviceService;

        private static readonly Guid ExistingUserId = new("11111111-1111-1111-1111-111111111111");
        private static readonly Guid ExistingDeviceModelId = new("22222222-2222-2222-2222-222222222222");
        private static readonly Guid ExistingDeviceId = new("33333333-3333-3333-3333-333333333333");
        private const string ExistingDeviceImei= "111111111111111";

        public DeviceServiceTests()
        {
            _mockDbContext = new Mock<ITrackingDbContext>();

            var users = new List<User> { new User { Id = ExistingUserId } };
            var deviceModels = new List<DeviceModel> { new DeviceModel { Id = ExistingDeviceModelId } };
            var devices = new List<Device> { new Device { Id = ExistingDeviceId, Imei = ExistingDeviceImei } };


            _mockDbContext.Setup(u => u.Users)
                .Returns(DbSetMockFactory.Create(users, u => u.Id).Object);

            _mockDbContext.Setup(u => u.Devices)
                .Returns(DbSetMockFactory.Create(devices, u => u.Id).Object);

            _mockDbContext.Setup(u => u.DeviceModel)
                .Returns(DbSetMockFactory.Create(deviceModels, u => u.Id).Object);

            _mockDbContext.Setup(d => d.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile<DeviceProfile>();
                }, NullLoggerFactory.Instance);
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }

            _deviceService = new DeviceService(_mockDbContext.Object, _mapper);
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

        [Fact]
        public async Task DeleteDeviceAsync_ValidDeviceId_ReturnsSuccess()
        {
            var device = new Device { Id = Guid.NewGuid() };
            var mockDevices = DbSetMockFactory.Create(new List<Device> { device }, x => x.Id);
            _mockDbContext.Setup(c => c.Devices).Returns(mockDevices.Object);

            var result = await _deviceService.DeleteDeviceAsync(device.Id);

            Assert.True(result.IsSuccess, result.Error?.ErrorMessage);
            mockDevices.Verify(s => s.Remove(device), Times.Once);
            _mockDbContext.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteDeviceAsync_InvalidDeviceId_ReturnsFailure()
        {
            var device = new Device { Id = Guid.NewGuid() };
            var mockDevices = DbSetMockFactory.Create(new List<Device> { new Device { Id = Guid.NewGuid() } }, x => x.Id);
            _mockDbContext.Setup(c => c.Devices).Returns(mockDevices.Object);

            var result = await _deviceService.DeleteDeviceAsync(device.Id);

            Assert.Equal(Errors.DeviceErrors.DeviceNotFound.ErrorMessage, result.Error?.ErrorMessage);
            mockDevices.Verify(s => s.Remove(It.IsAny<Device>()), Times.Never);
            _mockDbContext.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateDeviceAsync_ValidData_ReturnsSuccess()
        {
            var deviceId = Guid.NewGuid();
            var device = new Device { Id = deviceId, Imei = "123456789000000" };
            var mockDevices = DbSetMockFactory.Create(new List<Device> { device }, x => x.Id);
            _mockDbContext.Setup(c => c.Devices).Returns(mockDevices.Object);

            var updateDevice = new UpdateDeviceRequest
            {
                DeviceModelId = ExistingDeviceModelId.ToString(),
                Imei = "123456789000000"
            };
            var result = await _deviceService.UpdateDeviceAsync(deviceId, updateDevice);

            Assert.True(result.IsSuccess, result.Error?.ErrorMessage);
            mockDevices.Verify(s => s.Update(It.IsAny<Device>()), Times.Once);
            _mockDbContext.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateDeviceAsync_DeviceModelDoesNotExist_ReturnsErrorMessage()
        {
            var deviceId = Guid.NewGuid();
            var device = new Device { Id = deviceId, Imei = "123456789000000" };
            var mockDevices = DbSetMockFactory.Create(new List<Device> { device }, x => x.Id);
            _mockDbContext.Setup(c => c.Devices).Returns(mockDevices.Object);

            var updateDevice = new UpdateDeviceRequest
            {
                DeviceModelId = Guid.NewGuid().ToString(),
                Imei = "123456789000000"
            };
            var result = await _deviceService.UpdateDeviceAsync(deviceId, updateDevice);

            Assert.Equal(Errors.DeviceModelErrors.DeviceModelNotFound.ErrorMessage, result.Error?.ErrorMessage);
            mockDevices.Verify(s => s.Update(It.IsAny<Device>()), Times.Never);
            _mockDbContext.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateDeviceAsync_DeviceDoesNotExist_ReturnsErrorMessage()
        {
            var deviceId = Guid.NewGuid();
            var device = new Device { Id = Guid.NewGuid(), Imei = "123456789000000" };
            var mockDevices = DbSetMockFactory.Create(new List<Device> { device }, x => x.Id);
            _mockDbContext.Setup(c => c.Devices).Returns(mockDevices.Object);

            var updateDevice = new UpdateDeviceRequest
            {
                DeviceModelId = ExistingDeviceModelId.ToString(),
                Imei = "123456789000000"
            };
            var result = await _deviceService.UpdateDeviceAsync(deviceId, updateDevice);

            Assert.Equal(Errors.DeviceErrors.DeviceNotFound.ErrorMessage, result.Error?.ErrorMessage);
            mockDevices.Verify(s => s.Update(It.IsAny<Device>()), Times.Never);
            _mockDbContext.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }


        //[Fact]
        //public void MappingConfiguration_IsValid()
        //{
        //    var config = new MapperConfiguration(
        //        cfg => cfg.AddMaps(typeof(DeviceProfile).Assembly),
        //        NullLoggerFactory.Instance);

        //    config.AssertConfigurationIsValid();
        //}
    }
}
