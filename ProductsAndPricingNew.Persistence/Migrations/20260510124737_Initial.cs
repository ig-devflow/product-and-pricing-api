using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductsAndPricingNew.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Product");

            migrationBuilder.EnsureSchema(
                name: "ReferenceData");

            migrationBuilder.EnsureSchema(
                name: "PricingRef");

            migrationBuilder.CreateTable(
                name: "AccountCategory",
                schema: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    DivisionId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AddOn",
                schema: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DivisionId = table.Column<int>(type: "int", nullable: false),
                    UnitTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    AddOnTypeId = table.Column<int>(type: "int", nullable: false),
                    MinimumAge = table.Column<int>(type: "int", nullable: true),
                    AccountCategoryId = table.Column<int>(type: "int", nullable: true),
                    ProductCategoryId = table.Column<int>(type: "int", nullable: true),
                    OneToOneLessonsPerWeek = table.Column<int>(type: "int", nullable: true),
                    OfferingsClosureDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: false),
                    CostCentreCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GeneralLedgerCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddOn", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Audience",
                schema: "ReferenceData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audience", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContentTemplate",
                schema: "ReferenceData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Scope = table.Column<short>(type: "smallint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                schema: "ReferenceData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Course",
                schema: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DivisionId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CourseLanguageId = table.Column<int>(type: "int", nullable: false),
                    CourseIntensityId = table.Column<int>(type: "int", nullable: false),
                    UnitTypeId = table.Column<int>(type: "int", nullable: false),
                    MinimumAge = table.Column<int>(type: "int", nullable: true),
                    MinimumWeeks = table.Column<int>(type: "int", nullable: true),
                    AccountCategoryId = table.Column<int>(type: "int", nullable: true),
                    ProductCategoryId = table.Column<int>(type: "int", nullable: true),
                    OfferingsClosureDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: false),
                    CostCentreCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GeneralLedgerCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
                schema: "ReferenceData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsoCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Division",
                schema: "PricingRef",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TermsAndConditions = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    GroupsPaymentTerms = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    HeadOfficeEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HeadOfficeTelephoneNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BannerContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BannerData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    BannerFileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: false),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    District = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Street = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Division", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Package",
                schema: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DivisionId = table.Column<int>(type: "int", nullable: false),
                    UnitTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CommissionPercentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    MinimumAge = table.Column<int>(type: "int", nullable: true),
                    MaximumAge = table.Column<int>(type: "int", nullable: true),
                    MinimumWeeks = table.Column<int>(type: "int", nullable: true),
                    AccountCategoryId = table.Column<int>(type: "int", nullable: true),
                    ProductCategoryId = table.Column<int>(type: "int", nullable: true),
                    OfferingsClosureDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: false),
                    CostCentreCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GeneralLedgerCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Package", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategory",
                schema: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    DivisionId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transfer",
                schema: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DivisionId = table.Column<int>(type: "int", nullable: false),
                    UnitTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TransferTypeId = table.Column<int>(type: "int", nullable: false),
                    TransferPortId = table.Column<int>(type: "int", nullable: false),
                    TimeFrom = table.Column<TimeOnly>(type: "time", nullable: true),
                    TimeTo = table.Column<TimeOnly>(type: "time", nullable: true),
                    AccountCategoryId = table.Column<int>(type: "int", nullable: true),
                    ProductCategoryId = table.Column<int>(type: "int", nullable: true),
                    OfferingsClosureDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: false),
                    CostCentreCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GeneralLedgerCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransferPort",
                schema: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TransferPortTypeId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferPort", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DivisionTextContent",
                schema: "PricingRef",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DivisionId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                    Format = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    ContentTemplateId = table.Column<int>(type: "int", nullable: false),
                    AudienceId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DivisionTextContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DivisionTextContent_Audience_AudienceId",
                        column: x => x.AudienceId,
                        principalSchema: "ReferenceData",
                        principalTable: "Audience",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DivisionTextContent_ContentTemplate_ContentTemplateId",
                        column: x => x.ContentTemplateId,
                        principalSchema: "ReferenceData",
                        principalTable: "ContentTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DivisionTextContent_Division_DivisionId",
                        column: x => x.DivisionId,
                        principalSchema: "PricingRef",
                        principalTable: "Division",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PackageItem",
                schema: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductKind = table.Column<int>(type: "int", nullable: false),
                    ProductDefinitionId = table.Column<int>(type: "int", nullable: false),
                    Percentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    PackageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackageItem_Package_PackageId",
                        column: x => x.PackageId,
                        principalSchema: "Product",
                        principalTable: "Package",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransferPortInstruction",
                schema: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DivisionId = table.Column<int>(type: "int", nullable: false),
                    Instructions = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    TransferPortId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferPortInstruction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferPortInstruction_TransferPort_TransferPortId",
                        column: x => x.TransferPortId,
                        principalSchema: "Product",
                        principalTable: "TransferPort",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransferPortTerminal",
                schema: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    TransferPortId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferPortTerminal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferPortTerminal_TransferPort_TransferPortId",
                        column: x => x.TransferPortId,
                        principalSchema: "Product",
                        principalTable: "TransferPort",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountCategory_DivisionId_IsDeleted_IsActive",
                schema: "Product",
                table: "AccountCategory",
                columns: new[] { "DivisionId", "IsDeleted", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_AccountCategory_DivisionId_Name",
                schema: "Product",
                table: "AccountCategory",
                columns: new[] { "DivisionId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_AddOn_AccountCategoryId",
                schema: "Product",
                table: "AddOn",
                column: "AccountCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AddOn_AddOnTypeId",
                schema: "Product",
                table: "AddOn",
                column: "AddOnTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AddOn_DivisionId_Name",
                schema: "Product",
                table: "AddOn",
                columns: new[] { "DivisionId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_AddOn_ProductCategoryId",
                schema: "Product",
                table: "AddOn",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AddOn_UnitTypeId",
                schema: "Product",
                table: "AddOn",
                column: "UnitTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Audience_Name",
                schema: "ReferenceData",
                table: "Audience",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ContentTemplate_Name",
                schema: "ReferenceData",
                table: "ContentTemplate",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Country_Code",
                schema: "ReferenceData",
                table: "Country",
                column: "Code",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Country_Name",
                schema: "ReferenceData",
                table: "Country",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Course_DivisionId_Name",
                schema: "Product",
                table: "Course",
                columns: new[] { "DivisionId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Currency_IsoCode",
                schema: "ReferenceData",
                table: "Currency",
                column: "IsoCode",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Currency_Name",
                schema: "ReferenceData",
                table: "Currency",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Division_Name",
                schema: "PricingRef",
                table: "Division",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionTextContent_AudienceId",
                schema: "PricingRef",
                table: "DivisionTextContent",
                column: "AudienceId");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionTextContent_ContentTemplateId",
                schema: "PricingRef",
                table: "DivisionTextContent",
                column: "ContentTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_DivisionTextContent_DivisionId_ContentTemplateId_AudienceId",
                schema: "PricingRef",
                table: "DivisionTextContent",
                columns: new[] { "DivisionId", "ContentTemplateId", "AudienceId" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Package_AccountCategoryId",
                schema: "Product",
                table: "Package",
                column: "AccountCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Package_DivisionId_Name",
                schema: "Product",
                table: "Package",
                columns: new[] { "DivisionId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Package_ProductCategoryId",
                schema: "Product",
                table: "Package",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Package_UnitTypeId",
                schema: "Product",
                table: "Package",
                column: "UnitTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageItem_PackageId_ProductKind_ProductDefinitionId",
                schema: "Product",
                table: "PackageItem",
                columns: new[] { "PackageId", "ProductKind", "ProductDefinitionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategory_DivisionId_IsDeleted_IsActive",
                schema: "Product",
                table: "ProductCategory",
                columns: new[] { "DivisionId", "IsDeleted", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategory_DivisionId_Name",
                schema: "Product",
                table: "ProductCategory",
                columns: new[] { "DivisionId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Transfer_AccountCategoryId",
                schema: "Product",
                table: "Transfer",
                column: "AccountCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfer_DivisionId_Name",
                schema: "Product",
                table: "Transfer",
                columns: new[] { "DivisionId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Transfer_ProductCategoryId",
                schema: "Product",
                table: "Transfer",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfer_TransferPortId",
                schema: "Product",
                table: "Transfer",
                column: "TransferPortId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfer_TransferTypeId",
                schema: "Product",
                table: "Transfer",
                column: "TransferTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfer_UnitTypeId",
                schema: "Product",
                table: "Transfer",
                column: "UnitTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferPortInstruction_TransferPortId_DivisionId",
                schema: "Product",
                table: "TransferPortInstruction",
                columns: new[] { "TransferPortId", "DivisionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransferPortTerminal_TransferPortId_Number",
                schema: "Product",
                table: "TransferPortTerminal",
                columns: new[] { "TransferPortId", "Number" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountCategory",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "AddOn",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "Country",
                schema: "ReferenceData");

            migrationBuilder.DropTable(
                name: "Course",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "Currency",
                schema: "ReferenceData");

            migrationBuilder.DropTable(
                name: "DivisionTextContent",
                schema: "PricingRef");

            migrationBuilder.DropTable(
                name: "PackageItem",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "ProductCategory",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "Transfer",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "TransferPortInstruction",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "TransferPortTerminal",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "Audience",
                schema: "ReferenceData");

            migrationBuilder.DropTable(
                name: "ContentTemplate",
                schema: "ReferenceData");

            migrationBuilder.DropTable(
                name: "Division",
                schema: "PricingRef");

            migrationBuilder.DropTable(
                name: "Package",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "TransferPort",
                schema: "Product");
        }
    }
}
