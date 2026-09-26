using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Location.Tracking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UniqueImei : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Devices_Imei_UserId",
                table: "Devices");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_Imei",
                table: "Devices",
                column: "Imei",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Devices_Imei",
                table: "Devices");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_Imei_UserId",
                table: "Devices",
                columns: new[] { "Imei", "UserId" },
                unique: true);
        }
    }
}
