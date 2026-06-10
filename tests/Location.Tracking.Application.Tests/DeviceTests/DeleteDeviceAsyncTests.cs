using AutoMapper;
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
    public class DeleteDeviceAsyncTests
    {
        private readonly Mock<IDeviceRepository> _deviceRepository;
        private readonly Mock<IDeviceModelService> _deviceModelService;
        private readonly Mock<IMapper> _mapperMock;
        private readonly DeviceService _deviceService;
        public DeleteDeviceAsyncTests()
        {
            _deviceRepository = new Mock<IDeviceRepository>();
            _deviceModelService = new Mock<IDeviceModelService>();
            _mapperMock = new Mock<IMapper>();

            _deviceService = new DeviceService(_deviceRepository.Object, _deviceModelService.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task DeleteDeviceAsync_ValidData_ReturnsSuccessResult()
        {
            //Arrange
            Guid deviceId = Guid.NewGuid();

            Device device = new Device
            {
                Id = deviceId,
                Name = "Test"
            };

            _deviceRepository.Setup(d => d.GetByIdAsync(deviceId))
                .ReturnsAsync(device);

            _deviceRepository.Setup(d => d.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            //Act
            var response = await _deviceService.DeleteDeviceAsync(deviceId);

            //Assert
            Assert.True(response.IsSuccess);

            _deviceRepository.Verify(d => d.GetByIdAsync(deviceId), Times.Once);
            _deviceRepository.Verify(d => d.Delete(device), Times.Once);
            _deviceRepository.Verify(d => d.SaveChangesAsync(), Times.Once);
        }

        [Fact] 
        public async Task DeleteDeviceAsync_DeviceDoesNotExist_ReturnsFailure()
        {
            // Arrange
            var deviceId = Guid.NewGuid();

            _deviceRepository
                .Setup(r => r.GetByIdAsync(deviceId))
                .ReturnsAsync((Device)null);

            // Act
            var result = await _deviceService.DeleteDeviceAsync(deviceId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.DeviceErrors.DeviceNotFound, result.Error);

            _deviceRepository.Verify(r => r.Delete(It.IsAny<Device>()), Times.Never);
            _deviceRepository.Verify(r => r.SaveChangesAsync(), Times.Never);
        }
    }
}
