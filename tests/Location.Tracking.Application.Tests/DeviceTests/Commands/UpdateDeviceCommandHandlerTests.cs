using AutoMapper;
using Location.Tracking.Application.Devices.Commands.UpdateDevice;
using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Application.Shared;
using Location.Tracking.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Tests.DeviceTests.Commands
{
    public class UpdateDeviceCommandHandlerTests
    {
        private readonly Mock<IDeviceRepository> _deviceRepositoryMock;
        private readonly Mock<IDeviceModelRepository> _deviceModelRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateDeviceCommandHandler _handler;
        public UpdateDeviceCommandHandlerTests()
        {
            _deviceRepositoryMock = new Mock<IDeviceRepository>();
            _deviceModelRepositoryMock = new Mock<IDeviceModelRepository>();
            _mapperMock = new Mock<IMapper>();

            _handler = new UpdateDeviceCommandHandler(_deviceRepositoryMock.Object, _mapperMock.Object, _deviceModelRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ValidData_ReturnsSuccessResult()
        {
            //Arrange
            DeviceConfiguration deviceConfig = new DeviceConfiguration
            {
                DeviceModelName = "ModelA",
                Name = "Custom Name"
            };

            UpdateDeviceCommand command = new UpdateDeviceCommand
            {
                DeviceId = Guid.NewGuid(),
                DeviceConfiguration = deviceConfig
            };

            DeviceModel deviceModel = new DeviceModel
            {
                Id = Guid.NewGuid(),
                Name = "Test Model"
            };

            var device = new Device
            {
                Id = command.DeviceId,
                DeviceModelId = Guid.Empty,
                Name = "Real Name"
            };

            _deviceModelRepositoryMock.Setup(s => s.GetDeviceModelByName(deviceConfig.DeviceModelName))
                .ReturnsAsync(deviceModel);

            _deviceRepositoryMock
                .Setup(r => r.GetByIdAsync(command.DeviceId))
                .ReturnsAsync(device);

            _mapperMock
                .Setup(m => m.Map(deviceConfig, device))
                .Returns(device);

            _deviceRepositoryMock
                .Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            //Act
            var response = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.True(response.IsSuccess);

            _deviceModelRepositoryMock.Verify(dm => dm.GetDeviceModelByName(deviceConfig.DeviceModelName), Times.Once);
            _deviceRepositoryMock.Verify(d => d.GetByIdAsync(command.DeviceId), Times.Once);
            _mapperMock.Verify(d => d.Map(deviceConfig, device), Times.Once);
            _deviceRepositoryMock.Verify(d => d.Update(device), Times.Once);
            _deviceRepositoryMock.Verify(d => d.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_DeviceNotFound_ReturnsFailure()
        {
            // Arrange
            DeviceConfiguration deviceConfig = new DeviceConfiguration
            {
                DeviceModelName = "ModelA",
                Name = "Custom Name"
            };

            UpdateDeviceCommand command = new UpdateDeviceCommand
            {
                DeviceId = Guid.NewGuid(),
                DeviceConfiguration = deviceConfig
            };

            DeviceModel deviceModel = new DeviceModel
            {
                Id = Guid.NewGuid(),
                Name = "Test Model"
            };

            var deviceModelResult = Result<DeviceModel>.Success(new DeviceModel { Id = deviceModel.Id });

            _deviceModelRepositoryMock.Setup(s => s.GetDeviceModelByName(deviceConfig.DeviceModelName))
               .ReturnsAsync(deviceModel);

            _deviceRepositoryMock
                .Setup(r => r.GetByIdAsync(command.DeviceId))
                .ReturnsAsync((Device)null);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(response.IsSuccess);

            _deviceRepositoryMock.Verify(r => r.Update(It.IsAny<Device>()), Times.Never);
        }

        [Fact]
        public async Task Handle_DeviceModelNotFound_ReturnsFailure()
        {
            // Arrange
            DeviceConfiguration deviceConfig = new DeviceConfiguration
            {
                DeviceModelName = "ModelA",
                Name = "Custom Name"
            };

            UpdateDeviceCommand command = new UpdateDeviceCommand
            {
                DeviceId = Guid.NewGuid(),
                DeviceConfiguration = deviceConfig
            };

            DeviceModel deviceModel = new DeviceModel
            {
                Id = Guid.NewGuid(),
                Name = "Test Model"
            };

            _deviceModelRepositoryMock.Setup(s => s.GetDeviceModelByName(deviceConfig.DeviceModelName))
               .ReturnsAsync((DeviceModel)null);

            // Act
            var response = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(response.IsSuccess);
            Assert.NotNull(response.Error);
            Assert.Equal(response.Error, Errors.DeviceModelErrors.DeviceModelNotFound);
            _deviceRepositoryMock.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        }
    }
   
}
