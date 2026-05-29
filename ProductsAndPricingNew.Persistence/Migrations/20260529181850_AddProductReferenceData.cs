using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProductsAndPricingNew.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProductReferenceData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "HeadOfficeTelephoneNo",
                schema: "PricingRef",
                table: "Division",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "HeadOfficeEmail",
                schema: "PricingRef",
                table: "Division",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateTable(
                name: "CourseIntensity",
                schema: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseIntensity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseLanguage",
                schema: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseLanguage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransferType",
                schema: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferType", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "Product",
                table: "CourseIntensity",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Standard" },
                    { 2, "Semi Intensive" },
                    { 3, "Intensive" },
                    { 4, "ElectiveOnly" },
                    { 5, "Pre Intensive" },
                    { 6, "Fast Track Standard" },
                    { 7, "Fast Track Intensive" }
                });

            migrationBuilder.InsertData(
                schema: "Product",
                table: "CourseLanguage",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "English" },
                    { 2, "French" },
                    { 3, "Bilingual" },
                    { 4, "Maltese" }
                });

            migrationBuilder.InsertData(
                schema: "Product",
                table: "TransferType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Arrival" },
                    { 2, "Departure" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseIntensity_Name",
                schema: "Product",
                table: "CourseIntensity",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_CourseLanguage_Name",
                schema: "Product",
                table: "CourseLanguage",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_TransferType_Name",
                schema: "Product",
                table: "TransferType",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseIntensity",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "CourseLanguage",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "TransferType",
                schema: "Product");

            migrationBuilder.AlterColumn<string>(
                name: "HeadOfficeTelephoneNo",
                schema: "PricingRef",
                table: "Division",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HeadOfficeEmail",
                schema: "PricingRef",
                table: "Division",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
