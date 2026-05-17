using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductsAndPricingNew.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreateSchoolTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "School",
                schema: "PricingRef",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CentreId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LegacyCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MinimumStayInWeeks = table.Column<int>(type: "int", nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmergencyTelephone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FinanceCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LmsAccess = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DecommissionDate = table.Column<DateOnly>(type: "date", nullable: true),
                    AgeFrom = table.Column<short>(type: "smallint", nullable: true),
                    AgeTo = table.Column<short>(type: "smallint", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: false),
                    ContactCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContactCountryId = table.Column<int>(type: "int", nullable: true),
                    ContactDistrict = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContactPostalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContactStreet = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_School", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_School_Name",
                schema: "PricingRef",
                table: "School",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "School",
                schema: "PricingRef");
        }
    }
}
