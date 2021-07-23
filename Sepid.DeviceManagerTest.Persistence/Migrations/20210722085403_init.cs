using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sepid.DeviceManagerTest.Persistence.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeviceModels",
                columns: table => new
                {
                    deviceModelId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalLog = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    SdkType = table.Column<int>(type: "int", nullable: false),
                    IsFingerSupport = table.Column<bool>(type: "bit", nullable: false),
                    IsFaceSupport = table.Column<bool>(type: "bit", nullable: false),
                    IsCardSupport = table.Column<bool>(type: "bit", nullable: false),
                    IsPasswordSupport = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceModels", x => x.deviceModelId);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    GroupId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK_Groups_Groups_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    SettingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RetryFailedTransferNumber = table.Column<int>(type: "int", nullable: false),
                    FingerPrintQuality = table.Column<int>(type: "int", nullable: false),
                    AutoClearLogPercentage = table.Column<int>(type: "int", nullable: false),
                    EnableClearLog = table.Column<bool>(type: "bit", nullable: false),
                    License = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VitalDevice = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.SettingId);
                });

            migrationBuilder.CreateTable(
                name: "UserAccessGroups",
                columns: table => new
                {
                    UserGroupId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    GroupIds = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccessGroups", x => x.UserGroupId);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    DeviceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastRetrievedLogId = table.Column<long>(type: "bigint", nullable: true),
                    DeviceModelId = table.Column<long>(type: "bigint", nullable: true),
                    CurrentLogCount = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Serial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServerIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServerPort = table.Column<int>(type: "int", nullable: true),
                    Port = table.Column<int>(type: "int", nullable: false),
                    Gateway = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubnetMask = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVital = table.Column<bool>(type: "bit", nullable: false),
                    UseDhcp = table.Column<bool>(type: "bit", nullable: false),
                    IsDeviceToServer = table.Column<bool>(type: "bit", nullable: false),
                    ActiveImage = table.Column<bool>(type: "bit", nullable: false),
                    IsMatchOnServer = table.Column<bool>(type: "bit", nullable: false),
                    IsConnected = table.Column<bool>(type: "bit", nullable: false),
                    IsLock = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    EntranceMode = table.Column<int>(type: "int", nullable: false),
                    SyncLogStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLogRetrieve = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.DeviceId);
                    table.ForeignKey(
                        name: "FK_Devices_DeviceModels_DeviceModelId",
                        column: x => x.DeviceModelId,
                        principalTable: "DeviceModels",
                        principalColumn: "deviceModelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeviceInGroups",
                columns: table => new
                {
                    GroupId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceInGroups", x => new { x.GroupId, x.DeviceId });
                    table.ForeignKey(
                        name: "FK_DeviceInGroups_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeviceInGroups_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransferLogs",
                columns: table => new
                {
                    TransferLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeviceId = table.Column<int>(type: "int", nullable: true),
                    Retry = table.Column<int>(type: "int", nullable: false),
                    TransferLogType = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferLogs", x => x.TransferLogId);
                    table.ForeignKey(
                        name: "FK_TransferLogs_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeviceInGroups_DeviceId",
                table: "DeviceInGroups",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_DeviceModelId",
                table: "Devices",
                column: "DeviceModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_ParentId",
                table: "Groups",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferLogs_DeviceId",
                table: "TransferLogs",
                column: "DeviceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceInGroups");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "TransferLogs");

            migrationBuilder.DropTable(
                name: "UserAccessGroups");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "DeviceModels");
        }
    }
}
