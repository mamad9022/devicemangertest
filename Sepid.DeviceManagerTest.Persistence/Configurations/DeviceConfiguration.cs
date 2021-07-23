using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sepid.DeviceManagerTest.Application.Models;

namespace Sepid.DeviceManagerTest.Persistence.Configurations
{
    public class DeviceConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder.Property(e => e.Id)
                .HasColumnName("DeviceId").IsRequired().UseIdentityColumn();

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Ip).IsRequired();

            builder.Property(e => e.Serial).IsRequired();

            builder.Property(e => e.IsVital).IsRequired();

            builder.Property(e => e.Name).IsRequired();

            builder.Property(e => e.IsLock).IsRequired();

            builder.HasOne(e => e.DeviceModel)
                .WithMany(e => e.Devices)
                .HasForeignKey(e => e.DeviceModelId);
        }
    }
}