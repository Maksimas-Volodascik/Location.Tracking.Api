using Location.Tracking.Application.Shared.Interface;
using Location.Tracking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Location.Tracking.Infrastructure.Data
{
    public class TrackingDbContext : DbContext, ITrackingDbContext
    {
        public TrackingDbContext(DbContextOptions<TrackingDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<RawRecord> RawRecords { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceModel> DeviceModel { get; set; }
        public DbSet<LogEntry> LogEntry { get; set; }
        public DbSet<Workspace> Workspaces { get; set; }
        public DbSet<Invitation> Invitations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Device>()
                .HasOne(d => d.DeviceModel)
                .WithMany(dm => dm.Devices)
                .HasForeignKey(d => d.DeviceModelId)
                .IsRequired();

            modelBuilder.Entity<Device>()
                .HasMany(d => d.Records)
                .WithOne(r => r.Device)
                .HasForeignKey(d => d.DeviceId)
                .IsRequired();

            modelBuilder.Entity<Device>()
                .HasIndex(d => d.Imei)
                .IsUnique();

            modelBuilder.Entity<LogEntry>()
               .HasIndex(le => new {le.TraceId, le.ReceivedDate})
               .IsDescending(false, true);//order by newest

            modelBuilder.Entity<Workspace>()
                .HasMany(w => w.Users)
                .WithOne(u => u.Workspace)
                .HasForeignKey(u => u.WorkspaceId)
                .IsRequired();

            modelBuilder.Entity<Workspace>()
                .HasMany(w => w.Invitations)
                .WithOne(i => i.Workspace)
                .HasForeignKey(i => i.WorkspaceId)
                .IsRequired();

            modelBuilder.Entity<Workspace>()
                .HasMany(w => w.Devices)
                .WithOne(d => d.Workspace)
                .HasForeignKey(d => d.WorkspaceId)
                .IsRequired();

        }

    }
}
