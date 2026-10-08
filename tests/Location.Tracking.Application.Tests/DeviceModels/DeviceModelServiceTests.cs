using AutoMapper;
using Location.Tracking.Application.AutoMapper;
using Location.Tracking.Application.DeviceModels;
using Location.Tracking.Application.DeviceModels.Dtos;
using Location.Tracking.Application.Shared.Interface;
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

            _deviceModelService = new DeviceModelService(_mockDbContext.Object, _mapper);
        }

        [Fact]
        public async Task CreateNewDeviceModelAsync_ValidData_ReturnsSuccess()
        {
            var deviceModelRequest = new CreateDeviceModelRequest { Name = "FMC650(Teltonika)", ProtocolName = "FMC650" };
            var modelList = new List<DeviceModel> { new DeviceModel { Id = Guid.NewGuid(), Name = "FMC650(Teltonika)", ProtocolName = "FMC650" } };
            var modelDbSet = DbSetMockFactory.Create(modelList, e => e.Id);

            _mockDbContext.Setup(u => u.DeviceModel)
                .Returns(DbSetMockFactory.Create(modelList, u => u.Id).Object);

            var result = await _deviceModelService.CreateNewDeviceModelAsync(deviceModelRequest);

            Assert.True(result.IsSuccess, result.Error?.ErrorMessage);
            _mockDbContext.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
