using Location.Tracking.Application.Interfaces.Repositories;
using Location.Tracking.Domain.Entities;
using Location.Tracking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Infrastructure.Repositories
{
    public class DeviceModelRepository : BaseRepository<DeviceModel>, IDeviceModelRepository
    {
        private readonly TrackingDbContext _context;
        public DeviceModelRepository(TrackingDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DeviceModel>> GetAllDeviceModelsAsync()
        {
            var deviceModels = from deviceModel in _context.DeviceModel
                               select deviceModel;
            //todo: pagination
            return await deviceModels.ToListAsync();
        }

        public async Task<DeviceModel?> GetDeviceModelByName(string name)
        {
            var deviceModel = await _context.Set<DeviceModel>().FirstOrDefaultAsync(e => e.ProtocolName.Equals(name));

            return deviceModel;
        }
    }
}
