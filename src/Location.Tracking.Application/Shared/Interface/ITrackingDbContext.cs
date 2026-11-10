using Location.Tracking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Location.Tracking.Application.Shared.Interface
{
    public interface ITrackingDbContext
    {
        DbSet<User> Users { get; }
        DbSet<RawRecord> RawRecords { get; }
        DbSet<LogEntry> LogEntry { get; }
        DbSet<DeviceModel> DeviceModel { get; }
        DbSet<Device> Devices { get; }
        DbSet<Workspace> Workspaces { get; }
        DbSet<Invitation> Invitations { get; }
        DatabaseFacade Database { get; } 
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
