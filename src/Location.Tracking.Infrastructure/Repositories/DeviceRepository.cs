using Location.Tracking.Application.DTOs.Devices;
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
    public class DeviceRepository : BaseRepository<Device>, IDeviceRepository
    {
        private readonly TrackingDbContext _context;
        public DeviceRepository(TrackingDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Device>> GetAllDevicesAsync()
        {
            var deviceQuery = from device in _context.Devices
                              select device;

            return await deviceQuery.ToListAsync();
        }

        public async Task<DevicesMetrics> GetDeviceMetricsAsync()
        {
            var weekly = DateTimeOffset.UtcNow.AddDays(-7);

            var query = new DevicesMetrics
            {
                Total = await _context.Devices.CountAsync(),
                Weekly = await _context.Devices.CountAsync(d => d.DateAdded >= weekly)
            };

            return query;
        }
    }
}
