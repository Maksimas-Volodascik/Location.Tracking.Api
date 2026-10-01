using Location.Tracking.Application.Shared.Interface;
using Location.Tracking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(d => d.Devices)
                .WithOne(u => u.User)
                .HasForeignKey(d => d.UserId)
                .IsRequired();

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
        }

    }
}
