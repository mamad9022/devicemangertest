using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sepid.DeviceManagerTest.Application.Models;

namespace Sepid.DeviceManagerTest.Persistence.Configurations
{
    public class TransferLogConfiguration : IEntityTypeConfiguration<TransferLog>
    {
        public void Configure(EntityTypeBuilder<TransferLog> builder)
        {
            builder.Property(e => e.Id).IsRequired().HasColumnName("TransferLogId");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Retry).IsRequired();

            builder.Property(e => e.Description).IsRequired();

            builder.Property(e => e.IsSuccess).IsRequired();

            builder.Property(e => e.CreateDate).IsRequired();

            builder.HasOne(e => e.Device)
                .WithMany(e => e.TransferLogs)
                .HasForeignKey(e => e.DeviceId).OnDelete(DeleteBehavior.Cascade);



        }
    }
}