using AutoMapper;
using Location.Tracking.Application.Devices.Commands.CreateNewDevice;
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
    public class CreateNewDeviceCommandHandlerTests
    {
        private readonly Mock<IDeviceRepository> _deviceRepositoryMock;
        private readonly Mock<IDeviceModelRepository> _deviceModelRepositoryMock;
        private readonly CreateNewDeviceCommandHandler _handler;

        public CreateNewDeviceCommandHandlerTests()
        {
            _deviceRepositoryMock = new Mock<IDeviceRepository>();
            _deviceModelRepositoryMock = new Mock<IDeviceModelRepository>();
            _handler = new CreateNewDeviceCommandHandler(_deviceRepositoryMock.Object, _deviceModelRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ValidData_ReturnsSuccessResult()
        {
            //Arrange
            string userGuid = "1111a1a1-a1a1-11a1-11aa-1a1111aaaaaa";

            DeviceModel deviceModel = new DeviceModel
            {
                Id = new Guid("1111a1a1-a1a1-11a1-11aa-1a1111aaaaaa")
            };

            CreateNewDeviceCommand deviceConfiguration = new CreateNewDeviceCommand
            {
                DeviceModelName = "test-model",
                Imei = "012345678901234",
                IsEnabled = true,
                Name = "test_name"
            };

            Device newDevice = new Device
            {
                Imei = deviceConfiguration.Imei,
                IsEnabled = deviceConfiguration.IsEnabled,
                UserId = new Guid(userGuid),
                DeviceModelId = deviceModel.Id
            };

            _deviceModelRepositoryMock.Setup(dm => dm.GetDeviceModelByName(deviceConfiguration.DeviceModelName))
                .ReturnsAsync(deviceModel);

            _deviceRepositoryMock.Setup(d => d.AddAsync(It.IsAny<Device>()))
                .Returns(Task.CompletedTask);

            _deviceRepositoryMock.Setup(d => d.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            //Act
            var result = await _handler.Handle(deviceConfiguration, CancellationToken.None);

            //Assert
            Assert.True(result.IsSuccess);
            Assert.Null(result.Error);

            _deviceModelRepositoryMock.Verify(dm => dm.GetDeviceModelByName(deviceConfiguration.DeviceModelName), Times.Once);
            _deviceRepositoryMock.Verify(d => d.AddAsync(It.IsAny<Device>()), Times.Once);
            _deviceRepositoryMock.Verify(d => d.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidDeviceModel_ReturnsErrorResult()
        {
            //Arrange
            CreateNewDeviceCommand deviceConfiguration = new CreateNewDeviceCommand
            {
                DeviceModelName = "test-model",
                Imei = "012345678901234",
                IsEnabled = true,
                Name = "test_name"
            };

            _deviceModelRepositoryMock.Setup(dm => dm.GetDeviceModelByName(deviceConfiguration.DeviceModelName))
                .ReturnsAsync((DeviceModel)null);

            //Act
            var result = await _handler.Handle(deviceConfiguration, CancellationToken.None);

            //Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(result.Error, Errors.DeviceModelErrors.DeviceModelNotFound);

            _deviceModelRepositoryMock.Verify(dm => dm.GetDeviceModelByName(deviceConfiguration.DeviceModelName), Times.Once);
            _deviceRepositoryMock.Verify(d => d.SaveChangesAsync(), Times.Never);
        }

        //unit test to check user ID
    }
}
