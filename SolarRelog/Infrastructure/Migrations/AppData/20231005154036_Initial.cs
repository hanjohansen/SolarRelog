using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarRelog.Infrastructure.Migrations.AppData
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "devices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Ip = table.Column<string>(type: "TEXT", nullable: false),
                    Port = table.Column<int>(type: "INTEGER", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_devices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AppLogSettings_RetentionDays = table.Column<int>(type: "INTEGER", nullable: false),
                    AppLogSettings_MinLogLevel = table.Column<string>(type: "TEXT", nullable: false),
                    DataLogSettings_RetentionDays = table.Column<int>(type: "INTEGER", nullable: false),
                    DataLogSettings_PollingIntervalSeconds = table.Column<int>(type: "INTEGER", nullable: false),
                    InfluxSettings_Url = table.Column<string>(type: "TEXT", nullable: false),
                    InfluxSettings_Organization = table.Column<string>(type: "TEXT", nullable: false),
                    InfluxSettings_Bucket = table.Column<string>(type: "TEXT", nullable: false),
                    InfluxSettings_ApiToken = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "log_data",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    DeviceId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RecordDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LoggedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Pac = table.Column<decimal>(type: "TEXT", nullable: false),
                    Pdc = table.Column<decimal>(type: "TEXT", nullable: false),
                    Uac = table.Column<decimal>(type: "TEXT", nullable: false),
                    Udc = table.Column<decimal>(type: "TEXT", nullable: false),
                    YieldDay = table.Column<decimal>(type: "TEXT", nullable: false),
                    YieldYesterday = table.Column<decimal>(type: "TEXT", nullable: false),
                    YieldMonth = table.Column<decimal>(type: "TEXT", nullable: false),
                    YieldYear = table.Column<decimal>(type: "TEXT", nullable: false),
                    YieldTotal = table.Column<decimal>(type: "TEXT", nullable: false),
                    ConsPac = table.Column<decimal>(type: "TEXT", nullable: false),
                    ConsYieldDay = table.Column<decimal>(type: "TEXT", nullable: false),
                    ConsYieldYesterday = table.Column<decimal>(type: "TEXT", nullable: false),
                    ConsYieldMonth = table.Column<decimal>(type: "TEXT", nullable: false),
                    ConsYieldYear = table.Column<decimal>(type: "TEXT", nullable: false),
                    ConsYieldTotal = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalPower = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_log_data", x => x.Id);
                    table.ForeignKey(
                        name: "FK_log_data_devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "log_consumer_data",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    LogDataId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ConsumerIndex = table.Column<string>(type: "TEXT", nullable: false),
                    Consumption = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_log_consumer_data", x => x.Id);
                    table.ForeignKey(
                        name: "FK_log_consumer_data_log_data_LogDataId",
                        column: x => x.LogDataId,
                        principalTable: "log_data",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_log_consumer_data_LogDataId",
                table: "log_consumer_data",
                column: "LogDataId");

            migrationBuilder.CreateIndex(
                name: "IX_log_data_DeviceId",
                table: "log_data",
                column: "DeviceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "log_consumer_data");

            migrationBuilder.DropTable(
                name: "settings");

            migrationBuilder.DropTable(
                name: "log_data");

            migrationBuilder.DropTable(
                name: "devices");
        }
    }
}
