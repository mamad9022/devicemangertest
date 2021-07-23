using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sepid.DeviceManagerTest.Application.Models;

namespace Sepid.DeviceManagerTest.Persistence.Configurations
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.Property(e => e.Id).IsRequired().HasColumnName("GroupId");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name).IsRequired();

            builder.Property(e => e.CreateDate).IsRequired();

            builder.Property(e => e.IsDeleted).IsRequired();

            builder.HasQueryFilter(e => e.IsDeleted == false);

            builder.HasOne(e => e.Parent).WithMany(e => e.Children).HasForeignKey(e => e.ParentId);

        }
    }
}