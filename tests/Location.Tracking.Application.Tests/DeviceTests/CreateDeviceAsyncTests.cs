using AutoMapper;
using Location.Tracking.Application.DTOs;
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
    public class CreateDeviceAsyncTests
    {
        private readonly Mock<IDeviceRepository> _deviceRepositoryMock;
        private readonly IDeviceService _deviceService;
        private readonly Mock<IDeviceModelService> _deviceModelServiceMock;
        private readonly Mock<IUserService> _userServiceMock;

        public CreateDeviceAsyncTests()
        {
            _deviceRepositoryMock = new Mock<IDeviceRepository>();
            _deviceModelServiceMock = new Mock<IDeviceModelService>();
            _userServiceMock = new Mock<IUserService>();
            var mapperMock = new Mock<IMapper>(); //not used for tests

            _deviceService = new DeviceService(_deviceRepositoryMock.Object, _deviceModelServiceMock.Object, _userServiceMock.Object, mapperMock.Object);
        }

        [Fact]
        public async Task CreateDeviceAsync_ValidData_ReturnsSuccessResult()
        {
            //Arrange
            string userGuid = "1111a1a1-a1a1-11a1-11aa-1a1111aaaaaa";

            DeviceModel deviceModel = new DeviceModel
            {
                Id = new Guid("1111a1a1-a1a1-11a1-11aa-1a1111aaaaaa")
            };

            DeviceConfigurationDto deviceConfiguration = new DeviceConfigurationDto
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

            _deviceModelServiceMock.Setup(dm => dm.GetDeviceModelByNameAsync(deviceConfiguration.DeviceModelName))
                .ReturnsAsync(Result<DeviceModel>.Success(deviceModel));

            _deviceRepositoryMock.Setup(d => d.AddAsync(newDevice))
                .Returns(Task.CompletedTask);

            _deviceRepositoryMock.Setup(d => d.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            //Act
            var result = await _deviceService.CreateNewDeviceAsync(deviceConfiguration, userGuid);

            //Assert
            Assert.True(result.IsSuccess);
            _deviceModelServiceMock.Verify(dm => dm.GetDeviceModelByNameAsync(deviceConfiguration.DeviceModelName), Times.Once);
            _deviceRepositoryMock.Verify(d => d.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateDeviceAsync_InvalidDeviceModel_ReturnsErrorResult()
        {
            //Arrange
            string userGuid = "1111a1a1-a1a1-11a1-11aa-1a1111aaaaaa";

            DeviceConfigurationDto deviceConfiguration = new DeviceConfigurationDto
            {
                DeviceModelName = "test-model",
                Imei = "012345678901234",
                IsEnabled = true,
                Name = "test_name"
            };

            _deviceModelServiceMock.Setup(dm => dm.GetDeviceModelByNameAsync(deviceConfiguration.DeviceModelName))
                .ReturnsAsync(Result<DeviceModel>.Failure(Errors.DeviceModelErrors.DeviceModelNotFound));

            //Act
            var result = await _deviceService.CreateNewDeviceAsync(deviceConfiguration, userGuid);

            //Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(result.Error, Errors.DeviceModelErrors.DeviceModelNotFound);
            _deviceModelServiceMock.Verify(dm => dm.GetDeviceModelByNameAsync(deviceConfiguration.DeviceModelName), Times.Once);
            _deviceRepositoryMock.Verify(d => d.SaveChangesAsync(), Times.Never);
        }

        //unit test to check user ID
    }
}
