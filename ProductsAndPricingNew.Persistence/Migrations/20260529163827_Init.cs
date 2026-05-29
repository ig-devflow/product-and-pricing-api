using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductsAndPricingNew.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Product");

            migrationBuilder.EnsureSchema(
                name: "ReferenceData");

            migrationBuilder.EnsureSchema(
                name: "FeeOrDiscount");

            migrationBuilder.EnsureSchema(
                name: "PricingRef");

            migrationBuilder.EnsureSchema(
                name: "Edit");

            migrationBuilder.EnsureSchema(
                name: "Pricing");

            migrationBuilder.EnsureSchema(
                name: "Rules");

            migrationBuilder.EnsureSchema(
                name: "Tax");

            migrationBuilder.CreateTable(
                name: "Accommodation",
                schema: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AccommodationTypeId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    MinimumStayInWeeks = table.Column<int>(type: "int", nullable: false),
                    IsCommitted = table.Column<bool>(type: "bit", nullable: false),
                    IsNonCommitted = table.Column<bool>(type: "bit", nullable: false),
                    MaximumAge = table.Column<short>(type: "smallint", nullable: true),
                    MinimumAge = table.Column<short>(type: "smallint", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accommodation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccommodationBathroomType",
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
                    table.PrimaryKey("PK_AccommodationBathroomType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccommodationBoardType",
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
                    table.PrimaryKey("PK_AccommodationBoardType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccommodationRoomGrade",
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
                    table.PrimaryKey("PK_AccommodationRoomGrade", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccommodationRoomType",
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
                    table.PrimaryKey("PK_AccommodationRoomType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccommodationType",
                schema: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Prefix = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccommodationType", x => x.Id);
                });

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
                name: "CancellationFee",
                schema: "FeeOrDiscount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    DivisionId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: false),
                    AccountCategoryId = table.Column<int>(type: "int", nullable: false),
                    ProductCategoryId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CancellationFee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CentreContactType",
                schema: "PricingRef",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CentreContactType", x => x.Id);
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
                name: "Discount",
                schema: "FeeOrDiscount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DivisionId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: false),
                    ShouldApplyRulesetId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discount", x => x.Id);
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
                    HeadOfficeEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HeadOfficeTelephoneNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BannerContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BannerData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    BannerFileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: false),
                    ContactCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContactCountryId = table.Column<int>(type: "int", nullable: true),
                    ContactDistrict = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContactPostalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContactStreet = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Division", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Editor",
                schema: "Edit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Editor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fee",
                schema: "FeeOrDiscount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UnitTypeId = table.Column<int>(type: "int", nullable: false),
                    DivisionId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: false),
                    CostCentreCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GeneralLedgerCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PricingYear",
                schema: "Pricing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DivisionId = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: false),
                    EarlyBirdRulesetId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricingYear", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrintFormat",
                schema: "PricingRef",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrintFormat", x => x.Id);
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
                name: "ProductOffering",
                schema: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolId = table.Column<int>(type: "int", nullable: false),
                    UnitTypeId = table.Column<int>(type: "int", nullable: false),
                    UnitTypeIsDateBased = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: false),
                    DateStrategiesClosureDate = table.Column<DateOnly>(type: "date", nullable: true),
                    DecommissionDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ProductDefinitionId = table.Column<int>(type: "int", nullable: false),
                    ProductKind = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOffering", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ruleset",
                schema: "Rules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SubjectType = table.Column<int>(type: "int", nullable: false),
                    DivisionId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RuleVersion = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ruleset", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "School",
                schema: "PricingRef",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CentreId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MinimumStayInWeeks = table.Column<int>(type: "int", nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmergencyTelephone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FinanceCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LmsAccess = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DecommissionDate = table.Column<DateOnly>(type: "date", nullable: true),
                    MaximumAge = table.Column<short>(type: "smallint", nullable: true),
                    MinimumAge = table.Column<short>(type: "smallint", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: false),
                    ContactCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContactCountryId = table.Column<int>(type: "int", nullable: true),
                    ContactDistrict = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContactPostalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContactStreet = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_School", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaxRegimen",
                schema: "Tax",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ValidFrom = table.Column<DateOnly>(type: "date", nullable: false),
                    ValidTo = table.Column<DateOnly>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: false),
                    ScopeKind = table.Column<int>(type: "int", nullable: false),
                    ScopeTargetId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxRegimen", x => x.Id);
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
                name: "UnitType",
                schema: "ReferenceData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsDateBased = table.Column<bool>(type: "bit", nullable: false),
                    CalculationKind = table.Column<int>(type: "int", nullable: false),
                    MinMajorUnits = table.Column<int>(type: "int", nullable: false),
                    MinMinorUnits = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CancellationFeeOffering",
                schema: "FeeOrDiscount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolId = table.Column<int>(type: "int", nullable: false),
                    CancellationFeeId = table.Column<int>(type: "int", nullable: false),
                    ApplyRulesetId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: false),
                    ChargeRulesetId = table.Column<int>(type: "int", nullable: false),
                    ActiveFromPricingYear = table.Column<int>(type: "int", nullable: false),
                    ActiveToPricingYear = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CancellationFeeOffering", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CancellationFeeOffering_CancellationFee_CancellationFeeId",
                        column: x => x.CancellationFeeId,
                        principalSchema: "FeeOrDiscount",
                        principalTable: "CancellationFee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccommodationRoom",
                schema: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccommodationId = table.Column<int>(type: "int", nullable: false),
                    DivisionId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UnitTypeId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    OccupyRoom = table.Column<bool>(type: "bit", nullable: false),
                    OfferingsClosureDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: false),
                    AccountCategoryId = table.Column<int>(type: "int", nullable: false),
                    ProductCategoryId = table.Column<int>(type: "int", nullable: false),
                    CostCentreCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GeneralLedgerCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BathroomTypeId = table.Column<int>(type: "int", nullable: false),
                    BoardTypeId = table.Column<int>(type: "int", nullable: false),
                    RoomGradeId = table.Column<int>(type: "int", nullable: false),
                    RoomTypeId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccommodationRoom", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccommodationRoom_Accommodation_AccommodationId",
                        column: x => x.AccommodationId,
                        principalSchema: "Product",
                        principalTable: "Accommodation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccommodationRoom_Division_DivisionId",
                        column: x => x.DivisionId,
                        principalSchema: "PricingRef",
                        principalTable: "Division",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    OneToOneLessonsPerWeek = table.Column<int>(type: "int", nullable: true),
                    OfferingsClosureDate = table.Column<DateOnly>(type: "date", nullable: false),
                    MaximumAge = table.Column<short>(type: "smallint", nullable: true),
                    MinimumAge = table.Column<short>(type: "smallint", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: false),
                    AccountCategoryId = table.Column<int>(type: "int", nullable: false),
                    ProductCategoryId = table.Column<int>(type: "int", nullable: false),
                    CostCentreCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GeneralLedgerCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddOn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AddOn_Division_DivisionId",
                        column: x => x.DivisionId,
                        principalSchema: "PricingRef",
                        principalTable: "Division",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    MinimumWeeks = table.Column<int>(type: "int", nullable: true),
                    OfferingsClosureDate = table.Column<DateOnly>(type: "date", nullable: false),
                    MaximumAge = table.Column<short>(type: "smallint", nullable: true),
                    MinimumAge = table.Column<short>(type: "smallint", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: false),
                    AccountCategoryId = table.Column<int>(type: "int", nullable: false),
                    ProductCategoryId = table.Column<int>(type: "int", nullable: false),
                    CostCentreCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GeneralLedgerCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Course_Division_DivisionId",
                        column: x => x.DivisionId,
                        principalSchema: "PricingRef",
                        principalTable: "Division",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    MinimumWeeks = table.Column<int>(type: "int", nullable: true),
                    OfferingsClosureDate = table.Column<DateOnly>(type: "date", nullable: false),
                    MaximumAge = table.Column<short>(type: "smallint", nullable: true),
                    MinimumAge = table.Column<short>(type: "smallint", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: false),
                    AccountCategoryId = table.Column<int>(type: "int", nullable: false),
                    ProductCategoryId = table.Column<int>(type: "int", nullable: false),
                    CommissionPercentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    CostCentreCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GeneralLedgerCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Package", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Package_Division_DivisionId",
                        column: x => x.DivisionId,
                        principalSchema: "PricingRef",
                        principalTable: "Division",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    OfferingsClosureDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: false),
                    AccountCategoryId = table.Column<int>(type: "int", nullable: false),
                    ProductCategoryId = table.Column<int>(type: "int", nullable: false),
                    CostCentreCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GeneralLedgerCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TimeFrom = table.Column<TimeOnly>(type: "time", nullable: true),
                    TimeTo = table.Column<TimeOnly>(type: "time", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transfer_Division_DivisionId",
                        column: x => x.DivisionId,
                        principalSchema: "PricingRef",
                        principalTable: "Division",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FeeOffering",
                schema: "FeeOrDiscount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolId = table.Column<int>(type: "int", nullable: false),
                    FeeId = table.Column<int>(type: "int", nullable: false),
                    FeeOfferingTypeId = table.Column<int>(type: "int", nullable: false),
                    ApplicableRowRulesetId = table.Column<int>(type: "int", nullable: false),
                    ApplyRulesetId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: false),
                    ActiveFromPricingYear = table.Column<int>(type: "int", nullable: false),
                    ActiveToPricingYear = table.Column<int>(type: "int", nullable: true),
                    HasSpecificDates = table.Column<bool>(type: "bit", nullable: true),
                    MaximumUnits = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeOffering", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeeOffering_Fee_FeeId",
                        column: x => x.FeeId,
                        principalSchema: "FeeOrDiscount",
                        principalTable: "Fee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                    PrintFormatId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsPhysicalCentre = table.Column<bool>(type: "bit", nullable: false),
                    GeneralEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AccommodationEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Telephone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmergencyTelephone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TransferEmergencyTelephone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BrandColor = table.Column<string>(type: "nchar(7)", fixedLength: true, maxLength: 7, nullable: false),
                    SchoolSponsorshipNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VatNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VatExemptionNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ChequePayableTo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Guarantees = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    IndividualsRatio = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    StaffingRatio = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    EmptyBeds = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
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
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Centre", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Centre_PrintFormat_PrintFormatId",
                        column: x => x.PrintFormatId,
                        principalSchema: "PricingRef",
                        principalTable: "PrintFormat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OfferingYearPricing",
                schema: "Pricing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductOfferingId = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedById = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferingYearPricing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfferingYearPricing_ProductOffering_ProductOfferingId",
                        column: x => x.ProductOfferingId,
                        principalSchema: "Product",
                        principalTable: "ProductOffering",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleSegment",
                schema: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduleKind = table.Column<int>(type: "int", nullable: false),
                    From = table.Column<DateOnly>(type: "date", nullable: false),
                    To = table.Column<DateOnly>(type: "date", nullable: true),
                    RecurrenceWeekdayMask = table.Column<byte>(type: "tinyint", nullable: false),
                    ProductOfferingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleSegment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleSegment_ProductOffering_ProductOfferingId",
                        column: x => x.ProductOfferingId,
                        principalSchema: "Product",
                        principalTable: "ProductOffering",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PricingRule",
                schema: "Rules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RulesetId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ScriptJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricingRule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PricingRule_Ruleset_RulesetId",
                        column: x => x.RulesetId,
                        principalSchema: "Rules",
                        principalTable: "Ruleset",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaxBand",
                schema: "Tax",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaxRegimenId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TaxCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    SequenceKey = table.Column<int>(type: "int", nullable: false),
                    BandRulesetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxBand", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxBand_TaxRegimen_TaxRegimenId",
                        column: x => x.TaxRegimenId,
                        principalSchema: "Tax",
                        principalTable: "TaxRegimen",
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

            migrationBuilder.CreateTable(
                name: "PackageItem",
                schema: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductKind = table.Column<int>(type: "int", nullable: false),
                    ProductDefinitionId = table.Column<int>(type: "int", nullable: false),
                    PriceBreakdown = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
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
                name: "OneOffFeePrice",
                schema: "FeeOrDiscount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FeeOfferingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OneOffFeePrice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OneOffFeePrice_FeeOffering_FeeOfferingId",
                        column: x => x.FeeOfferingId,
                        principalSchema: "FeeOrDiscount",
                        principalTable: "FeeOffering",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupplementPrice",
                schema: "FeeOrDiscount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    PricePerMajorUnit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PricePerMinorUnit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PeriodStart = table.Column<DateOnly>(type: "date", nullable: true),
                    PeriodEnd = table.Column<DateOnly>(type: "date", nullable: true),
                    FeeOfferingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplementPrice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplementPrice_FeeOffering_FeeOfferingId",
                        column: x => x.FeeOfferingId,
                        principalSchema: "FeeOrDiscount",
                        principalTable: "FeeOffering",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CentreContacts",
                schema: "PricingRef",
                columns: table => new
                {
                    ContactTypeId = table.Column<int>(type: "int", nullable: false),
                    CentreId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SignatureData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    SignatureContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SignatureFileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CentreContacts", x => new { x.CentreId, x.ContactTypeId });
                    table.ForeignKey(
                        name: "FK_CentreContacts_CentreContactType_ContactTypeId",
                        column: x => x.ContactTypeId,
                        principalSchema: "PricingRef",
                        principalTable: "CentreContactType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.CreateTable(
                name: "PricePeriod",
                schema: "Pricing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Start = table.Column<DateOnly>(type: "date", nullable: false),
                    End = table.Column<DateOnly>(type: "date", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    BandingMethod = table.Column<int>(type: "int", nullable: false),
                    OfferingYearPricingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricePeriod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PricePeriod_OfferingYearPricing_OfferingYearPricingId",
                        column: x => x.OfferingYearPricingId,
                        principalSchema: "Pricing",
                        principalTable: "OfferingYearPricing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleSegmentDate",
                schema: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Start = table.Column<DateOnly>(type: "date", nullable: false),
                    End = table.Column<DateOnly>(type: "date", nullable: true),
                    ScheduleSegmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleSegmentDate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleSegmentDate_ScheduleSegment_ScheduleSegmentId",
                        column: x => x.ScheduleSegmentId,
                        principalSchema: "Product",
                        principalTable: "ScheduleSegment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PriceBand",
                schema: "Pricing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinUnits = table.Column<int>(type: "int", nullable: false),
                    MaxUnits = table.Column<int>(type: "int", nullable: false),
                    PricePerUnit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PricePeriodId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceBand", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceBand_PricePeriod_PricePeriodId",
                        column: x => x.PricePeriodId,
                        principalSchema: "Pricing",
                        principalTable: "PricePeriod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Edit",
                table: "Editor",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "UserName" },
                values: new object[] { 1, "noreply@ecenglish.com", "System", "User", "SYSTEM" });

            migrationBuilder.CreateIndex(
                name: "IX_Accommodation_Name",
                schema: "Product",
                table: "Accommodation",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationBathroomType_Name",
                schema: "Product",
                table: "AccommodationBathroomType",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationBoardType_Name",
                schema: "Product",
                table: "AccommodationBoardType",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationRoom_AccommodationId",
                schema: "Product",
                table: "AccommodationRoom",
                column: "AccommodationId");

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationRoom_DivisionId_Name",
                schema: "Product",
                table: "AccommodationRoom",
                columns: new[] { "DivisionId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationRoomGrade_Name",
                schema: "Product",
                table: "AccommodationRoomGrade",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationRoomType_Name",
                schema: "Product",
                table: "AccommodationRoomType",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationType_Name",
                schema: "Product",
                table: "AccommodationType",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

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
                name: "IX_Audience_Name",
                schema: "ReferenceData",
                table: "Audience",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_CancellationFee_DivisionId_Name",
                schema: "FeeOrDiscount",
                table: "CancellationFee",
                columns: new[] { "DivisionId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_CancellationFeeOffering_CancellationFeeId",
                schema: "FeeOrDiscount",
                table: "CancellationFeeOffering",
                column: "CancellationFeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CancellationFeeOffering_SchoolId_CancellationFeeId",
                schema: "FeeOrDiscount",
                table: "CancellationFeeOffering",
                columns: new[] { "SchoolId", "CancellationFeeId" });

            migrationBuilder.CreateIndex(
                name: "IX_Centre_Code",
                schema: "PricingRef",
                table: "Centre",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Centre_Name",
                schema: "PricingRef",
                table: "Centre",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Centre_PrintFormatId",
                schema: "PricingRef",
                table: "Centre",
                column: "PrintFormatId");

            migrationBuilder.CreateIndex(
                name: "IX_CentreContacts_ContactTypeId",
                schema: "PricingRef",
                table: "CentreContacts",
                column: "ContactTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CentreContactType_Name",
                schema: "PricingRef",
                table: "CentreContactType",
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
                name: "IX_Discount_DivisionId_Type",
                schema: "FeeOrDiscount",
                table: "Discount",
                columns: new[] { "DivisionId", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_Division_Name",
                schema: "PricingRef",
                table: "Division",
                column: "Name",
                unique: true);

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
                name: "IX_Fee_DivisionId_Name",
                schema: "FeeOrDiscount",
                table: "Fee",
                columns: new[] { "DivisionId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_FeeOffering_FeeId",
                schema: "FeeOrDiscount",
                table: "FeeOffering",
                column: "FeeId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeOffering_SchoolId_FeeId",
                schema: "FeeOrDiscount",
                table: "FeeOffering",
                columns: new[] { "SchoolId", "FeeId" });

            migrationBuilder.CreateIndex(
                name: "IX_OfferingYearPricing_ProductOfferingId_Year",
                schema: "Pricing",
                table: "OfferingYearPricing",
                columns: new[] { "ProductOfferingId", "Year" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OneOffFeePrice_FeeOfferingId_Year_CurrencyId",
                schema: "FeeOrDiscount",
                table: "OneOffFeePrice",
                columns: new[] { "FeeOfferingId", "Year", "CurrencyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Package_DivisionId_Name",
                schema: "Product",
                table: "Package",
                columns: new[] { "DivisionId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_PackageItem_PackageId_ProductKind_ProductDefinitionId",
                schema: "Product",
                table: "PackageItem",
                columns: new[] { "PackageId", "ProductKind", "ProductDefinitionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PriceBand_PricePeriodId",
                schema: "Pricing",
                table: "PriceBand",
                column: "PricePeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_PricePeriod_OfferingYearPricingId",
                schema: "Pricing",
                table: "PricePeriod",
                column: "OfferingYearPricingId");

            migrationBuilder.CreateIndex(
                name: "IX_PricingRule_RulesetId_Name",
                schema: "Rules",
                table: "PricingRule",
                columns: new[] { "RulesetId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PricingRule_RulesetId_Priority",
                schema: "Rules",
                table: "PricingRule",
                columns: new[] { "RulesetId", "Priority" });

            migrationBuilder.CreateIndex(
                name: "IX_PricingYear_DivisionId_Year",
                schema: "Pricing",
                table: "PricingYear",
                columns: new[] { "DivisionId", "Year" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrintFormat_Name",
                schema: "PricingRef",
                table: "PrintFormat",
                column: "Name",
                unique: true,
                filter: "[IsDeleted] = 0");

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
                name: "IX_ProductOffering_SchoolId",
                schema: "Product",
                table: "ProductOffering",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Ruleset_DivisionId_SubjectType_Status",
                schema: "Rules",
                table: "Ruleset",
                columns: new[] { "DivisionId", "SubjectType", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleSegment_ProductOfferingId",
                schema: "Product",
                table: "ScheduleSegment",
                column: "ProductOfferingId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleSegmentDate_ScheduleSegmentId",
                schema: "Product",
                table: "ScheduleSegmentDate",
                column: "ScheduleSegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_School_Name",
                schema: "PricingRef",
                table: "School",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupplementPrice_FeeOfferingId",
                schema: "FeeOrDiscount",
                table: "SupplementPrice",
                column: "FeeOfferingId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxBand_TaxRegimenId_SequenceKey",
                schema: "Tax",
                table: "TaxBand",
                columns: new[] { "TaxRegimenId", "SequenceKey" });

            migrationBuilder.CreateIndex(
                name: "IX_TaxBand_TaxRegimenId_TaxCode",
                schema: "Tax",
                table: "TaxBand",
                columns: new[] { "TaxRegimenId", "TaxCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaxRegimen_ValidFrom",
                schema: "Tax",
                table: "TaxRegimen",
                column: "ValidFrom");

            migrationBuilder.CreateIndex(
                name: "IX_Transfer_DivisionId_Name",
                schema: "Product",
                table: "Transfer",
                columns: new[] { "DivisionId", "Name" });

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
                name: "AccommodationBathroomType",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "AccommodationBoardType",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "AccommodationRoom",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "AccommodationRoomGrade",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "AccommodationRoomType",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "AccommodationType",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "AccountCategory",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "AddOn",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "CancellationFeeOffering",
                schema: "FeeOrDiscount");

            migrationBuilder.DropTable(
                name: "CentreContacts",
                schema: "PricingRef");

            migrationBuilder.DropTable(
                name: "CentreTextContent",
                schema: "PricingRef");

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
                name: "Discount",
                schema: "FeeOrDiscount");

            migrationBuilder.DropTable(
                name: "DivisionTextContent",
                schema: "PricingRef");

            migrationBuilder.DropTable(
                name: "Editor",
                schema: "Edit");

            migrationBuilder.DropTable(
                name: "OneOffFeePrice",
                schema: "FeeOrDiscount");

            migrationBuilder.DropTable(
                name: "PackageItem",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "PriceBand",
                schema: "Pricing");

            migrationBuilder.DropTable(
                name: "PricingRule",
                schema: "Rules");

            migrationBuilder.DropTable(
                name: "PricingYear",
                schema: "Pricing");

            migrationBuilder.DropTable(
                name: "ProductCategory",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "ScheduleSegmentDate",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "School",
                schema: "PricingRef");

            migrationBuilder.DropTable(
                name: "SupplementPrice",
                schema: "FeeOrDiscount");

            migrationBuilder.DropTable(
                name: "TaxBand",
                schema: "Tax");

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
                name: "UnitType",
                schema: "ReferenceData");

            migrationBuilder.DropTable(
                name: "Accommodation",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "CancellationFee",
                schema: "FeeOrDiscount");

            migrationBuilder.DropTable(
                name: "CentreContactType",
                schema: "PricingRef");

            migrationBuilder.DropTable(
                name: "Centre",
                schema: "PricingRef");

            migrationBuilder.DropTable(
                name: "Audience",
                schema: "ReferenceData");

            migrationBuilder.DropTable(
                name: "ContentTemplate",
                schema: "ReferenceData");

            migrationBuilder.DropTable(
                name: "Package",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "PricePeriod",
                schema: "Pricing");

            migrationBuilder.DropTable(
                name: "Ruleset",
                schema: "Rules");

            migrationBuilder.DropTable(
                name: "ScheduleSegment",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "FeeOffering",
                schema: "FeeOrDiscount");

            migrationBuilder.DropTable(
                name: "TaxRegimen",
                schema: "Tax");

            migrationBuilder.DropTable(
                name: "TransferPort",
                schema: "Product");

            migrationBuilder.DropTable(
                name: "PrintFormat",
                schema: "PricingRef");

            migrationBuilder.DropTable(
                name: "Division",
                schema: "PricingRef");

            migrationBuilder.DropTable(
                name: "OfferingYearPricing",
                schema: "Pricing");

            migrationBuilder.DropTable(
                name: "Fee",
                schema: "FeeOrDiscount");

            migrationBuilder.DropTable(
                name: "ProductOffering",
                schema: "Product");
        }
    }
}
