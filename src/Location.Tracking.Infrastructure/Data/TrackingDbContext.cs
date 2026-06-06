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
    public class TrackingDbContext : DbContext
    {
        public TrackingDbContext(DbContextOptions<TrackingDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<RawRecord> RawRecords { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceModel> DeviceModel { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Devices)
                .WithOne(d => d.User)
                .HasForeignKey(u => u.UserId)
                .IsRequired();

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
                .HasIndex(d => new { d.Imei, d.UserId })
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }

    }
}
