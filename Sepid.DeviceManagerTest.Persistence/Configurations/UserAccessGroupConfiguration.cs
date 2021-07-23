using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using Sepid.DeviceManagerTest.Application.Models;

namespace Sepid.DeviceManagerTest.Persistence.Configurations
{
    public class UserAccessGroupConfiguration : IEntityTypeConfiguration<UserAccessGroup>
    {
        public void Configure(EntityTypeBuilder<UserAccessGroup> builder)
        {
            builder.Property(e => e.Id).IsRequired().HasColumnName("UserGroupId").ValueGeneratedNever();

            builder.HasKey(e => e.Id);

            builder.Property(e => e.UserId).IsRequired();

            builder.Property(e => e.IsDeleted).IsRequired();

            builder.HasQueryFilter(e => e.IsDeleted == false);

            builder.Property(e => e.GroupIds)
                .IsRequired()
                .HasConversion(
                    v => JsonConvert.SerializeObject(v,
                        new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                    v => JsonConvert.DeserializeObject<List<long>>(v,
                        new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
        }
    }
}