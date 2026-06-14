using Location.Tracking.Application.Devices.Commands.DeleteDevice;
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
    public class DeleteDeviceCommandHandlerTests
    {
        private readonly Mock<IDeviceRepository> _deviceRepositoryMock;
        private readonly DeleteDeviceCommandHandler _handler;
        public DeleteDeviceCommandHandlerTests()
        {
            _deviceRepositoryMock = new Mock<IDeviceRepository>();

            _handler = new DeleteDeviceCommandHandler(_deviceRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ValidData_ReturnsSuccessResult()
        {
            //Arrange
            DeleteDeviceCommand command = new DeleteDeviceCommand
            {
                DeviceId = Guid.NewGuid()
            };
                

            Device device = new Device
            {
                Id = command.DeviceId,
                Name = "Test"
            };

            _deviceRepositoryMock.Setup(d => d.GetByIdAsync(command.DeviceId))
                .ReturnsAsync(device);

            _deviceRepositoryMock.Setup(d => d.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            //Act
            var response = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.True(response.IsSuccess);
            Assert.Null(response.Error);

            _deviceRepositoryMock.Verify(d => d.GetByIdAsync(command.DeviceId), Times.Once);
            _deviceRepositoryMock.Verify(d => d.Delete(device), Times.Once);
            _deviceRepositoryMock.Verify(d => d.SaveChangesAsync(), Times.Once);
        }

        [Fact] 
        public async Task Handle_DeviceDoesNotExist_ReturnsFailure()
        {
            // Arrange
            DeleteDeviceCommand command = new DeleteDeviceCommand
            {
                DeviceId = Guid.NewGuid()
            };

            _deviceRepositoryMock
                .Setup(r => r.GetByIdAsync(command.DeviceId))
                .ReturnsAsync((Device)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.DeviceErrors.DeviceNotFound, result.Error);

            _deviceRepositoryMock.Verify(r => r.Delete(It.IsAny<Device>()), Times.Never);
            _deviceRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }
    }
}
