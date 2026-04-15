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
                .HasMany(e => e.Devices)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .IsRequired();

            modelBuilder.Entity<Device>()
                .HasOne(e => e.DeviceModel)
                .WithOne(e => e.Device)
                .HasForeignKey<Device>(e => e.DeviceModelId)
                .IsRequired();

            modelBuilder.Entity<Device>()
                .HasMany(e => e.Records)
                .WithOne(e => e.Device)
                .HasForeignKey(e => e.DeviceId)
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
