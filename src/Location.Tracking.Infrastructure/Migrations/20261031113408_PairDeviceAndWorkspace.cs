using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Location.Tracking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PairDeviceAndWorkspace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Users_UserId",
                table: "Devices");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Devices",
                newName: "WorkspaceId");

            migrationBuilder.RenameIndex(
                name: "IX_Devices_UserId",
                table: "Devices",
                newName: "IX_Devices_WorkspaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Workspaces_WorkspaceId",
                table: "Devices",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Workspaces_WorkspaceId",
                table: "Devices");

            migrationBuilder.RenameColumn(
                name: "WorkspaceId",
                table: "Devices",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Devices_WorkspaceId",
                table: "Devices",
                newName: "IX_Devices_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Users_UserId",
                table: "Devices",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
