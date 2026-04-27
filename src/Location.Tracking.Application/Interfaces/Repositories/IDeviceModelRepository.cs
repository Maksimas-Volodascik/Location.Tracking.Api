using Location.Tracking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Interfaces.Repositories
{
    public interface IDeviceModelRepository : IBaseRepository<DeviceModel>
    {
        Task<DeviceModel?> GetDeviceModelByName(string name);
        Task<IEnumerable<DeviceModel>> GetAllDeviceModelsAsync();
    }
}
