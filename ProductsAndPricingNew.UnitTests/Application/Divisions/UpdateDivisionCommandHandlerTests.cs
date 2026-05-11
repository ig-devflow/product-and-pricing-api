using MediatR;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Features.Division.Commands.UpdateDivision;
using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.ReferenceData;
using ProductsAndPricingNew.Domain.SharedKernel.TextContent;
using ProductsAndPricingNew.UnitTests.TestSupport.Assertions;
using ProductsAndPricingNew.UnitTests.TestSupport.Builders;
using ProductsAndPricingNew.UnitTests.TestSupport.Fakes;

namespace ProductsAndPricingNew.UnitTests.Application.Divisions;

public sealed class UpdateDivisionCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenDivisionNotFound_ReturnsNotFound()
    {
        UpdateDivisionCommandHandler handler = CreateHandler();

        var result = await handler.Handle(new UpdateDivisionCommandBuilder().Build(), CancellationToken.None);

        ResultAssertions.AssertFailedWith<NotFoundError>(result);
    }

    [Fact]
    public async Task Handle_WhenVersionDoesNotMatch_ReturnsConflict()
    {
        byte[] currentVersion = [1, 2, 3, 4, 5, 6, 7, 8];
        byte[] staleVersion = [8, 7, 6, 5, 4, 3, 2, 1];

        Division division = new DivisionBuilder()
            .WithVersion(currentVersion)
            .Build();

        DivisionRepositoryFake repository = new DivisionRepositoryFake()
            .WithDivision(1, division);

        UnitOfWorkFake unitOfWork = new();
        UpdateDivisionCommandHandler handler = CreateHandler(repository, unitOfWork: unitOfWork);

        UpdateDivisionCommand command = new UpdateDivisionCommandBuilder()
            .WithId(1)
            .WithVersion(staleVersion)
            .Build();

        var result = await handler.Handle(command, CancellationToken.None);

        ResultAssertions.AssertFailedWith<ConflictError>(result);
        Assert.Equal(0, unitOfWork.SaveChangesCalls);
    }

    [Fact]
    public async Task Handle_WhenNameBelongsToAnotherDivision_ReturnsConflict()
    {
        DivisionRepositoryFake repository = new DivisionRepositoryFake()
            .WithDivision(1, new DivisionBuilder().Build());

        DivisionQueryFake query = new DivisionQueryFake()
            .WithExistingName("Division", 2);

        UpdateDivisionCommandHandler handler = CreateHandler(repository, query);

        var result = await handler.Handle(new UpdateDivisionCommandBuilder().WithId(1).Build(), CancellationToken.None);

        ResultAssertions.AssertFailedWith<ConflictError>(result);
    }

    [Fact]
    public async Task Handle_WhenValidCommand_UpdatesDivision()
    {
        Division division = new DivisionBuilder().Build();

        DivisionRepositoryFake repository = new DivisionRepositoryFake()
            .WithDivision(1, division);

        UpdateDivisionCommandHandler handler = CreateHandler(repository);

        UpdateDivisionCommand command = new UpdateDivisionCommandBuilder()
            .WithId(1)
            .WithName("Updated division")
            .WithWebsiteUrl("https://updated.example.com")
            .WithTermsAndConditions("Updated terms")
            .WithGroupsPaymentTerms("Updated payment terms")
            .WithHeadOfficeEmail("updated@example.com")
            .WithHeadOfficeTelephoneNo("+1 555 987 6543")
            .WithContactAddress(new AddressDtoBuilder().WithCountryId(2).WithStreet("New street").Build())
            .WithAccreditationBanner(new ImageBannerDtoBuilder().WithContentType("image/webp").WithFileName("updated.webp").Build())
            .Build();

        command = command with { IsActive = false };

        var result = await handler.Handle(command, CancellationToken.None);

        ResultAssertions.AssertSucceeded(result);
        Assert.Equal(Unit.Value, result.Value);
        Assert.Equal("Updated division", division.Name);
        Assert.False(division.IsActive);
        Assert.Equal("https://updated.example.com", division.WebsiteUrl);
        Assert.Equal("Updated terms", division.TermsAndConditions);
        Assert.Equal("Updated payment terms", division.GroupsPaymentTerms);
        Assert.Equal("updated@example.com", division.HeadOfficeEmail);
        Assert.Equal("+1 555 987 6543", division.HeadOfficeTelephoneNo);
        Assert.Equal(2, division.ContactAddress.CountryId);
        Assert.Equal("New street", division.ContactAddress.Street);
        Assert.Equal("image/webp", division.AccreditationBanner.ContentType);
    }

    [Fact]
    public async Task Handle_WhenValidCommand_ReplacesTexts()
    {
        Division division = new DivisionBuilder()
            .WithTexts(new TextContentDefinition(100, null, "Old text", ContentFormat.PlainText))
            .Build();

        DivisionRepositoryFake repository = new DivisionRepositoryFake()
            .WithDivision(1, division);

        UpdateDivisionCommandHandler handler = CreateHandler(repository);

        UpdateDivisionCommand command = new UpdateDivisionCommandBuilder()
            .WithId(1)
            .WithTexts(new TextContentDtoBuilder().WithContentTemplateId(101).WithContent("New text").Build())
            .Build();

        await handler.Handle(command, CancellationToken.None);

        Assert.Contains(division.Texts, text => text.ContentTemplateId == 100 && text.IsDeleted);
        Assert.Contains(division.Texts, text => text.ContentTemplateId == 101 && !text.IsDeleted && text.Text.Content == "New text");
    }

    [Fact]
    public async Task Handle_WhenContactAddressIsNull_ResetsAddress()
    {
        Division division = new DivisionBuilder()
            .WithContactAddress(1, "Street")
            .Build();

        DivisionRepositoryFake repository = new DivisionRepositoryFake()
            .WithDivision(1, division);

        UpdateDivisionCommandHandler handler = CreateHandler(repository);

        UpdateDivisionCommand command = new UpdateDivisionCommandBuilder()
            .WithId(1)
            .WithContactAddress(null)
            .Build();

        await handler.Handle(command, CancellationToken.None);

        Assert.Same(ProductsAndPricingNew.Domain.SharedKernel.ValueObjects.Address.Empty, division.ContactAddress);
    }

    [Fact]
    public async Task Handle_WhenBannerIsNull_ResetsBanner()
    {
        Division division = new DivisionBuilder()
            .WithAccreditationBanner([1], "image/png", "banner.png")
            .Build();

        DivisionRepositoryFake repository = new DivisionRepositoryFake()
            .WithDivision(1, division);

        UpdateDivisionCommandHandler handler = CreateHandler(repository);

        UpdateDivisionCommand command = new UpdateDivisionCommandBuilder()
            .WithId(1)
            .WithAccreditationBanner(null)
            .Build();

        await handler.Handle(command, CancellationToken.None);

        Assert.Same(ProductsAndPricingNew.Domain.SharedKernel.ValueObjects.ImageFile.Empty, division.AccreditationBanner);
    }

    [Fact]
    public async Task Handle_WhenValidCommand_CallsSaveChangesOnce()
    {
        DivisionRepositoryFake repository = new DivisionRepositoryFake()
            .WithDivision(1, new DivisionBuilder().Build());

        UnitOfWorkFake unitOfWork = new();
        UpdateDivisionCommandHandler handler = CreateHandler(repository, unitOfWork: unitOfWork);

        await handler.Handle(new UpdateDivisionCommandBuilder().WithId(1).Build(), CancellationToken.None);

        Assert.Equal(1, unitOfWork.SaveChangesCalls);
    }

    [Fact]
    public async Task Handle_WhenConflict_DoesNotSaveChanges()
    {
        DivisionRepositoryFake repository = new DivisionRepositoryFake()
            .WithDivision(1, new DivisionBuilder().Build());

        DivisionQueryFake query = new DivisionQueryFake()
            .WithExistingName("Division", 2);

        UnitOfWorkFake unitOfWork = new();
        UpdateDivisionCommandHandler handler = CreateHandler(repository, query, unitOfWork);

        await handler.Handle(new UpdateDivisionCommandBuilder().WithId(1).Build(), CancellationToken.None);

        Assert.Equal(0, unitOfWork.SaveChangesCalls);
    }

    private static UpdateDivisionCommandHandler CreateHandler(
        DivisionRepositoryFake? repository = null,
        DivisionQueryFake? query = null,
        UnitOfWorkFake? unitOfWork = null)
        => new(
            repository ?? new DivisionRepositoryFake(),
            query ?? new DivisionQueryFake(),
            unitOfWork ?? new UnitOfWorkFake());
}