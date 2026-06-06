using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Location.Tracking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MultipleModelDevice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Devices_DeviceModelId",
                table: "Devices");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_DeviceModelId",
                table: "Devices",
                column: "DeviceModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Devices_DeviceModelId",
                table: "Devices");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_DeviceModelId",
                table: "Devices",
                column: "DeviceModelId",
                unique: true);
        }
    }
}
