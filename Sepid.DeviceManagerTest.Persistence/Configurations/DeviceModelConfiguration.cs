using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sepid.DeviceManagerTest.Application.Models;

namespace Sepid.DeviceManagerTest.Persistence.Configurations
{
    public class DeviceModelConfiguration : IEntityTypeConfiguration<DeviceModel>
    {
        public void Configure(EntityTypeBuilder<DeviceModel> builder)
        {
            builder.Property(e => e.Id).HasColumnName("deviceModelId").IsRequired();

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Code).IsRequired();

            builder.Property(e => e.SdkType).IsRequired();

            builder.Property(e => e.Name).IsRequired();
        }
    }
}