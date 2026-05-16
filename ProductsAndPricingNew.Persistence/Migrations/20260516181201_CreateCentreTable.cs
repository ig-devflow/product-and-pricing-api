using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductsAndPricingNew.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreateCentreTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostCentreCode",
                schema: "Product",
                table: "Transfer");

            migrationBuilder.DropColumn(
                name: "GeneralLedgerCode",
                schema: "Product",
                table: "Transfer");

            migrationBuilder.DropColumn(
                name: "CostCentreCode",
                schema: "Product",
                table: "Package");

            migrationBuilder.DropColumn(
                name: "GeneralLedgerCode",
                schema: "Product",
                table: "Package");

            migrationBuilder.DropColumn(
                name: "CostCentreCode",
                schema: "Product",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "GeneralLedgerCode",
                schema: "Product",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "CostCentreCode",
                schema: "Product",
                table: "AddOn");

            migrationBuilder.DropColumn(
                name: "GeneralLedgerCode",
                schema: "Product",
                table: "AddOn");

            migrationBuilder.RenameTable(
                name: "TransferPortTerminal",
                schema: "Product",
                newName: "TransferPortTerminal");

            migrationBuilder.RenameTable(
                name: "TransferPortInstruction",
                schema: "Product",
                newName: "TransferPortInstruction");

            migrationBuilder.RenameTable(
                name: "PackageItem",
                schema: "Product",
                newName: "PackageItem");

            migrationBuilder.RenameColumn(
                name: "Street",
                schema: "PricingRef",
                table: "Division",
                newName: "ContactStreet");

            migrationBuilder.RenameColumn(
                name: "PostalCode",
                schema: "PricingRef",
                table: "Division",
                newName: "ContactPostalCode");

            migrationBuilder.RenameColumn(
                name: "District",
                schema: "PricingRef",
                table: "Division",
                newName: "ContactDistrict");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                schema: "PricingRef",
                table: "Division",
                newName: "ContactCountryId");

            migrationBuilder.RenameColumn(
                name: "City",
                schema: "PricingRef",
                table: "Division",
                newName: "ContactCity");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "Edit",
                table: "Editor",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

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
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Centre",
                schema: "PricingRef",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    PrintFormat = table.Column<short>(type: "smallint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsPhysicalCentre = table.Column<bool>(type: "bit", nullable: false),
                    GeneralEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AccommodationEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmergencyTelephone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TransferEmergencyTelephone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BrandColor = table.Column<string>(type: "nchar(7)", fixedLength: true, maxLength: 7, nullable: false),
                    SchoolSponsorshipNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VatNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VatExemptionNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ChequePayableTo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Guarantees = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IndividualsRatio = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StaffingRatio = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EmptyBeds = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: false),
                    BankAccountNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BankBeneficiaryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BankCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BankCountryId = table.Column<int>(type: "int", nullable: true),
                    BankDistrict = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BankPostalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BankStreet = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BeneficiaryCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BeneficiaryCountryId = table.Column<int>(type: "int", nullable: true),
                    BeneficiaryDistrict = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BeneficiaryPostalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BeneficiaryStreet = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AbaRoutingNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AchAba = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BranchCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Iban = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SwiftCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IntermediaryBankName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IntermediarySwiftCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IntermediaryCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IntermediaryCountryId = table.Column<int>(type: "int", nullable: true),
                    IntermediaryDistrict = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IntermediaryPostalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IntermediaryStreet = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContactCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContactCountryId = table.Column<int>(type: "int", nullable: true),
                    ContactDistrict = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContactPostalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContactStreet = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LogoContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LogoData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    LogoFileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Centre", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CentreContacts",
                schema: "PricingRef",
                columns: table => new
                {
                    ContactType = table.Column<short>(type: "smallint", nullable: false),
                    CentreId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SignatureData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    SignatureContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SignatureFileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CentreContacts", x => new { x.CentreId, x.ContactType });
                    table.ForeignKey(
                        name: "FK_CentreContacts_Centre_CentreId",
                        column: x => x.CentreId,
                        principalSchema: "PricingRef",
                        principalTable: "Centre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CentreTextContent",
                schema: "PricingRef",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CentreId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                    Format = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    ContentTemplateId = table.Column<int>(type: "int", nullable: false),
                    AudienceId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CentreTextContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CentreTextContent_Audience_AudienceId",
                        column: x => x.AudienceId,
                        principalSchema: "ReferenceData",
                        principalTable: "Audience",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CentreTextContent_Centre_CentreId",
                        column: x => x.CentreId,
                        principalSchema: "PricingRef",
                        principalTable: "Centre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CentreTextContent_ContentTemplate_ContentTemplateId",
                        column: x => x.ContentTemplateId,
                        principalSchema: "ReferenceData",
                        principalTable: "ContentTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Centre_Name",
                schema: "PricingRef",
                table: "Centre",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_CentreTextContent_AudienceId",
                schema: "PricingRef",
                table: "CentreTextContent",
                column: "AudienceId");

            migrationBuilder.CreateIndex(
                name: "IX_CentreTextContent_CentreId_ContentTemplateId_AudienceId",
                schema: "PricingRef",
                table: "CentreTextContent",
                columns: new[] { "CentreId", "ContentTemplateId", "AudienceId" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_CentreTextContent_ContentTemplateId",
                schema: "PricingRef",
                table: "CentreTextContent",
                column: "ContentTemplateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CentreContacts",
                schema: "PricingRef");

            migrationBuilder.DropTable(
                name: "CentreTextContent",
                schema: "PricingRef");

            migrationBuilder.DropTable(
                name: "Centre",
                schema: "PricingRef");

            migrationBuilder.RenameTable(
                name: "TransferPortTerminal",
                newName: "TransferPortTerminal",
                newSchema: "Product");

            migrationBuilder.RenameTable(
                name: "TransferPortInstruction",
                newName: "TransferPortInstruction",
                newSchema: "Product");

            migrationBuilder.RenameTable(
                name: "PackageItem",
                newName: "PackageItem",
                newSchema: "Product");

            migrationBuilder.RenameColumn(
                name: "ContactStreet",
                schema: "PricingRef",
                table: "Division",
                newName: "Street");

            migrationBuilder.RenameColumn(
                name: "ContactPostalCode",
                schema: "PricingRef",
                table: "Division",
                newName: "PostalCode");

            migrationBuilder.RenameColumn(
                name: "ContactDistrict",
                schema: "PricingRef",
                table: "Division",
                newName: "District");

            migrationBuilder.RenameColumn(
                name: "ContactCountryId",
                schema: "PricingRef",
                table: "Division",
                newName: "CountryId");

            migrationBuilder.RenameColumn(
                name: "ContactCity",
                schema: "PricingRef",
                table: "Division",
                newName: "City");

            migrationBuilder.AddColumn<string>(
                name: "CostCentreCode",
                schema: "Product",
                table: "Transfer",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GeneralLedgerCode",
                schema: "Product",
                table: "Transfer",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CostCentreCode",
                schema: "Product",
                table: "Package",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GeneralLedgerCode",
                schema: "Product",
                table: "Package",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "Edit",
                table: "Editor",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

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
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "CostCentreCode",
                schema: "Product",
                table: "Course",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GeneralLedgerCode",
                schema: "Product",
                table: "Course",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CostCentreCode",
                schema: "Product",
                table: "AddOn",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GeneralLedgerCode",
                schema: "Product",
                table: "AddOn",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
