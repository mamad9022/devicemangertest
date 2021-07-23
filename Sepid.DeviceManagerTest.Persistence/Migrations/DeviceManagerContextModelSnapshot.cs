﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sepid.DeviceManagerTest.Persistence.Context;

namespace Sepid.DeviceManagerTest.Persistence.Migrations
{
    [DbContext(typeof(DeviceManagerContext))]
    partial class DeviceManagerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Sepid.DeviceManagerTest.Application.Models.Device", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("DeviceId")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("ActiveImage")
                        .HasColumnType("bit");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateDateTime")
                        .HasColumnType("datetime2");

                    b.Property<long?>("CurrentLogCount")
                        .HasColumnType("bigint");

                    b.Property<long?>("DeviceModelId")
                        .HasColumnType("bigint");

                    b.Property<int>("EntranceMode")
                        .HasColumnType("int");

                    b.Property<string>("Gateway")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ip")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsConnected")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeviceToServer")
                        .HasColumnType("bit");

                    b.Property<bool>("IsLock")
                        .HasColumnType("bit");

                    b.Property<bool>("IsMatchOnServer")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVital")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastLogRetrieve")
                        .HasColumnType("datetime2");

                    b.Property<long?>("LastRetrievedLogId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Port")
                        .HasColumnType("int");

                    b.Property<string>("Serial")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ServerIp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ServerPort")
                        .HasColumnType("int");

                    b.Property<string>("SubnetMask")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SyncLogStartDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("UseDhcp")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("DeviceModelId");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("Sepid.DeviceManagerTest.Application.Models.DeviceInGroup", b =>
                {
                    b.Property<long>("GroupId")
                        .HasColumnType("bigint");

                    b.Property<int>("DeviceId")
                        .HasColumnType("int");

                    b.HasKey("GroupId", "DeviceId");

                    b.HasIndex("DeviceId");

                    b.ToTable("DeviceInGroups");
                });

            modelBuilder.Entity("Sepid.DeviceManagerTest.Application.Models.DeviceModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("deviceModelId")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Code")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsCardSupport")
                        .HasColumnType("bit");

                    b.Property<bool>("IsFaceSupport")
                        .HasColumnType("bit");

                    b.Property<bool>("IsFingerSupport")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPasswordSupport")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SdkType")
                        .HasColumnType("int");

                    b.Property<int>("TotalLog")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("DeviceModels");
                });

            modelBuilder.Entity("Sepid.DeviceManagerTest.Application.Models.Group", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("GroupId")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("ParentId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("Sepid.DeviceManagerTest.Application.Models.Setting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("SettingId")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AutoClearLogPercentage")
                        .HasColumnType("int");

                    b.Property<bool>("EnableClearLog")
                        .HasColumnType("bit");

                    b.Property<int>("FingerPrintQuality")
                        .HasColumnType("int");

                    b.Property<string>("License")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RetryFailedTransferNumber")
                        .HasColumnType("int");

                    b.Property<int>("VitalDevice")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("Sepid.DeviceManagerTest.Application.Models.TransferLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("TransferLogId");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Data")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("DeviceId")
                        .HasColumnType("int");

                    b.Property<string>("ErrorMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsSuccess")
                        .HasColumnType("bit");

                    b.Property<string>("Reason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Retry")
                        .HasColumnType("int");

                    b.Property<int>("TransferLogType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.ToTable("TransferLogs");
                });

            modelBuilder.Entity("Sepid.DeviceManagerTest.Application.Models.UserAccessGroup", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int")
                        .HasColumnName("UserGroupId");

                    b.Property<string>("GroupIds")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("UserAccessGroups");
                });

            modelBuilder.Entity("Sepid.DeviceManagerTest.Application.Models.Device", b =>
                {
                    b.HasOne("Sepid.DeviceManagerTest.Application.Models.DeviceModel", "DeviceModel")
                        .WithMany("Devices")
                        .HasForeignKey("DeviceModelId");

                    b.Navigation("DeviceModel");
                });

            modelBuilder.Entity("Sepid.DeviceManagerTest.Application.Models.DeviceInGroup", b =>
                {
                    b.HasOne("Sepid.DeviceManagerTest.Application.Models.Device", "Device")
                        .WithMany("DeviceInGroups")
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sepid.DeviceManagerTest.Application.Models.Group", "Group")
                        .WithMany("DeviceInGroups")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Device");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("Sepid.DeviceManagerTest.Application.Models.Group", b =>
                {
                    b.HasOne("Sepid.DeviceManagerTest.Application.Models.Group", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Sepid.DeviceManagerTest.Application.Models.TransferLog", b =>
                {
                    b.HasOne("Sepid.DeviceManagerTest.Application.Models.Device", "Device")
                        .WithMany("TransferLogs")
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Device");
                });

            modelBuilder.Entity("Sepid.DeviceManagerTest.Application.Models.Device", b =>
                {
                    b.Navigation("DeviceInGroups");

                    b.Navigation("TransferLogs");
                });

            modelBuilder.Entity("Sepid.DeviceManagerTest.Application.Models.DeviceModel", b =>
                {
                    b.Navigation("Devices");
                });

            modelBuilder.Entity("Sepid.DeviceManagerTest.Application.Models.Group", b =>
                {
                    b.Navigation("Children");

                    b.Navigation("DeviceInGroups");
                });
#pragma warning restore 612, 618
        }
    }
}