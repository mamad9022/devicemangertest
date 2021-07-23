using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sepid.DeviceManagerTest.Application.Models;

namespace Sepid.DeviceManagerTest.Persistence.Configurations
{
    public class DeviceInGroupConfiguration : IEntityTypeConfiguration<DeviceInGroup>
    {
        public void Configure(EntityTypeBuilder<DeviceInGroup> builder)
        {
            builder.HasKey(e => new { e.GroupId, e.DeviceId });

            builder.HasOne(e => e.Group)
                .WithMany(e => e.DeviceInGroups)
                .HasForeignKey(e => e.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Device)
                .WithMany(e => e.DeviceInGroups)
                .HasForeignKey(e => e.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}