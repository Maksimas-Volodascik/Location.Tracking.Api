using AutoMapper;
using Location.Tracking.Application.AutoMapper;
using Location.Tracking.Application.DeviceModels;
using Location.Tracking.Application.DeviceModels.Dtos;
using Location.Tracking.Application.Shared.Interface;
using Location.Tracking.Application.Shared.Results;
using Location.Tracking.Application.Tests.Shared;
using Location.Tracking.Domain.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
namespace Location.Tracking.Application.Tests.DeviceModels
{
    public class DeviceModelServiceTests
    {
        private readonly Mock<ITrackingDbContext> _mockDbContext;
        private readonly DeviceModelService _deviceModelService;
        private static IMapper _mapper;
        public DeviceModelServiceTests()
        {
            _mockDbContext = new Mock<ITrackingDbContext>();

            if(_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile<DeviceModelProfile>();
                }, NullLoggerFactory.Instance);
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }

            var modelList = new List<DeviceModel> { new DeviceModel { Id = Guid.NewGuid()} };
            _mockDbContext.Setup(u => u.DeviceModel).Returns(DbSetMockFactory.Create(modelList, x => x.Id).Object);

            _mockDbContext.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            _deviceModelService = new DeviceModelService(_mockDbContext.Object, _mapper);
        }

        [Fact]
        public async Task CreateNewDeviceModelAsync_ValidData_ReturnsSuccess()
        {
            var deviceModelRequest = new CreateDeviceModelRequest { Name = "Custom(test)", ProtocolName = "Custom" };

            var result = await _deviceModelService.CreateNewDeviceModelAsync(deviceModelRequest);

            Assert.True(result.IsSuccess, result.Error?.ErrorMessage);
            _mockDbContext.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteDeviceModelAsync_ValidData_ReturnsSuccess()
        {
            var deviceModel = new DeviceModel { Id = Guid.NewGuid() };
            var modelList = new List<DeviceModel> { deviceModel };
            var modelDbSet = DbSetMockFactory.Create(modelList, u => u.Id);

            _mockDbContext.Setup(u => u.DeviceModel)
                .Returns(modelDbSet.Object);

            var result = await _deviceModelService.DeleteDeviceModelAsync(deviceModel.Id);

            Assert.True(result.IsSuccess, result.Error?.ErrorMessage);
            modelDbSet.Verify(u => u.Remove(deviceModel), Times.Once);
            _mockDbContext.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteDeviceModelAsync_ModelIdDoesNotExist_ReturnsFailure()
        {
            var deviceModel = new DeviceModel { Id = Guid.NewGuid() };
            var modelDbSet = DbSetMockFactory.Create(new List<DeviceModel> { new DeviceModel { Id = Guid.NewGuid()} }, u => u.Id);
            _mockDbContext.Setup(u => u.DeviceModel)
                .Returns(modelDbSet.Object);

            var result = await _deviceModelService.DeleteDeviceModelAsync(deviceModel.Id);

            Assert.Equal(Errors.DeviceModelErrors.DeviceModelNotFound.ErrorMessage, result.Error?.ErrorMessage);
            modelDbSet.Verify(u => u.Remove(It.IsAny<DeviceModel>()), Times.Never);
            _mockDbContext.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateDeviceModelAsync_ValidData_ReturnsSuccess()
        {
            var updateDeviceModel = new UpdateDeviceModelRequest { Name = "FMC130(Teltonika)", ProtocolName = "FMC130" };

            var deviceModel = new DeviceModel { Id = Guid.NewGuid(), Name = "Eco5(Ruptela)", ProtocolName = "Eco5" };
            var modelList = new List<DeviceModel> { deviceModel };
            var modelDbSet = DbSetMockFactory.Create(modelList, u => u.Id);

            _mockDbContext.Setup(u => u.DeviceModel)
                .Returns(modelDbSet.Object);

            var result = await _deviceModelService.UpdateDeviceModelAsync(deviceModel.Id, updateDeviceModel);

            Assert.True(result.IsSuccess, result.Error?.ErrorMessage);
            Assert.Equal(deviceModel.ProtocolName, updateDeviceModel.ProtocolName);
            _mockDbContext.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateDeviceModelAsync_InvalidDeviceModelId_ReturnsFailure()
        {
            var updateDeviceModel = new UpdateDeviceModelRequest { Name = "FMC130(Teltonika)", ProtocolName = "FMC130" };

            var deviceModel = new DeviceModel { Id = Guid.NewGuid() };
            var modelList = new List<DeviceModel> { deviceModel };
            var modelDbSet = DbSetMockFactory.Create(modelList, u => u.Id);

            _mockDbContext.Setup(u => u.DeviceModel)
                .Returns(modelDbSet.Object);

            var result = await _deviceModelService.UpdateDeviceModelAsync(Guid.NewGuid(), updateDeviceModel);

            Assert.Equal(Errors.DeviceModelErrors.DeviceModelNotFound.ErrorMessage, result.Error?.ErrorMessage);
            modelDbSet.Verify(u => u.Update(It.IsAny<DeviceModel>()), Times.Never);
            _mockDbContext.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
