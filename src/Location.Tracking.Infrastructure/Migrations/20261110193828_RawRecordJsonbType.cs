using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Location.Tracking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RawRecordJsonbType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE "RawRecords"
                ALTER COLUMN "ParsedData"
                TYPE jsonb
                USING "ParsedData"::jsonb;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE "RawRecords"
                ALTER COLUMN "ParsedData"
                TYPE text
                USING "ParsedData"::text;
                """);
        }
    }
}
