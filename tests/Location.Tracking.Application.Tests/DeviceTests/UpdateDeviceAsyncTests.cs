using AutoMapper;
using Location.Tracking.Application.DTOs.Device;
using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Application.Interfaces.Services;
using Location.Tracking.Application.Services;
using Location.Tracking.Application.Shared;
using Location.Tracking.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Tests.DeviceTests
{
    public class UpdateDeviceAsyncTests
    {
        private readonly Mock<IDeviceRepository> _deviceRepository;
        private readonly Mock<IDeviceModelService> _deviceModelService;
        private readonly Mock<IMapper> _mapperMock;
        private readonly DeviceService _deviceService;
        public UpdateDeviceAsyncTests()
        {
            _deviceRepository = new Mock<IDeviceRepository>();
            _deviceModelService = new Mock<IDeviceModelService>();
            _mapperMock = new Mock<IMapper>();

            _deviceService = new DeviceService(_deviceRepository.Object, _deviceModelService.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task UpdateDeviceAsync_ValidData_ReturnsSuccessResult()
        {
            //Arrange
            var deviceId = Guid.NewGuid();
            var modelId = Guid.NewGuid();

            var deviceModelResult = Result<DeviceModel>.Success(new DeviceModel { Id = modelId });

            var dto = new DeviceConfigurationDto
            {
                DeviceModelName = "ModelA",
                Name = "Custom Name"
            };

            var device = new Device
            {
                Id = deviceId,
                DeviceModelId = Guid.Empty,
                Name = "Real Name"
            };

            _deviceModelService.Setup(s => s.GetDeviceModelByNameAsync(dto.DeviceModelName))
                .ReturnsAsync(deviceModelResult);

            _deviceRepository
                .Setup(r => r.GetByIdAsync(deviceId))
                .ReturnsAsync(device);

            _mapperMock
                .Setup(m => m.Map(dto, device))
                .Returns(device);

            _deviceRepository
                .Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);


            //Act
            var response = await _deviceService.UpdateDeviceAsync(dto, deviceId);

            //Assert
            Assert.True(response.IsSuccess);

            Assert.Equal(modelId, device.DeviceModelId);

            _deviceRepository.Verify(r => r.Update(device), Times.Once);
            _deviceRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateDeviceAsync_DeviceNotFound_ReturnsFailure()
        {
            // Arrange
            var deviceId = Guid.NewGuid();
            var modelId = Guid.NewGuid();

            var dto = new DeviceConfigurationDto
            {
                DeviceModelName = "ModelA"
            };

            var deviceModelResult = Result<DeviceModel>.Success(new DeviceModel { Id = modelId });

            _deviceModelService.Setup(s => s.GetDeviceModelByNameAsync(dto.DeviceModelName))
               .ReturnsAsync(deviceModelResult);

            _deviceRepository
                .Setup(r => r.GetByIdAsync(deviceId))
                .ReturnsAsync((Device)null);

            // Act
            var response = await _deviceService.UpdateDeviceAsync(dto, deviceId);

            // Assert
            Assert.False(response.IsSuccess);

            _deviceRepository.Verify(r => r.Update(It.IsAny<Device>()), Times.Never);
        }

        [Fact]
        public async Task UpdateDeviceAsync_DeviceModelNotFound_ReturnsFailure()
        {
            // Arrange
            var deviceId = Guid.NewGuid();

            var dto = new DeviceConfigurationDto
            {
                DeviceModelName = "ModelA"
            };

            _deviceModelService.Setup(s => s.GetDeviceModelByNameAsync(dto.DeviceModelName))
               .ReturnsAsync(Result<DeviceModel>.Failure(Errors.DeviceModelErrors.DeviceModelNotFound));

            // Act
            var response = await _deviceService.UpdateDeviceAsync(dto, deviceId);

            // Assert
            Assert.False(response.IsSuccess);

            _deviceRepository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        }
    }
   
}
