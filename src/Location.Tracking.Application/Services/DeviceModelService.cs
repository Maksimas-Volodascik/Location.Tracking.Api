using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Application.Interfaces.Services;
using Location.Tracking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Services
{
    public class DeviceModelService : IDeviceModelService
    {
        private readonly IDeviceModelRepository _deviceModelRepository;
        public DeviceModelService(IDeviceModelRepository deviceModelRepository)
        {
            _deviceModelRepository = deviceModelRepository;
        }

        public async Task<DeviceModel?> GetDeviceModelByName(string model)
        {
            var deviceModel = await _deviceModelRepository.GetDeviceModelByName(model);

            return deviceModel;
        }
    }
}
