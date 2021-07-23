using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sepid.DeviceManagerTest.Application.Models;

namespace Sepid.DeviceManagerTest.Persistence.Configurations
{
    public class SettingConfiguration : IEntityTypeConfiguration<Setting>
    {
        public void Configure(EntityTypeBuilder<Setting> builder)
        {
            builder.Property(e => e.Id).IsRequired().HasColumnName("SettingId");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.FingerPrintQuality).IsRequired();

            builder.Property(e => e.RetryFailedTransferNumber).IsRequired();
        }
    }
}