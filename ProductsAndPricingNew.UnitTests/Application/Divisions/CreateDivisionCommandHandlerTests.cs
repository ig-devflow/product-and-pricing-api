using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Features.Division.Commands.CreateDivision;
using ProductsAndPricingNew.UnitTests.TestSupport.Assertions;
using ProductsAndPricingNew.UnitTests.TestSupport.Builders;
using ProductsAndPricingNew.UnitTests.TestSupport.Fakes;

namespace ProductsAndPricingNew.UnitTests.Application.Divisions;

public sealed class CreateDivisionCommandHandlerTests
{
    [Fact]
    public async Task Handle_WithValidCommand_CreatesDivision()
    {
        DivisionRepositoryFake repository = new();
        CreateDivisionCommandHandler handler = CreateHandler(repository);
        CreateDivisionCommand command = new CreateDivisionCommandBuilder()
            .WithName("New division")
            .WithWebsiteUrl("https://division.example.com")
            .Build();

        await handler.Handle(command, CancellationToken.None);

        Assert.NotNull(repository.AddedDivision);
        Assert.Equal("New division", repository.AddedDivision.Name);
        Assert.Equal("https://division.example.com", repository.AddedDivision.WebsiteUrl);
        Assert.Single(repository.AddedDivision.Texts);
    }

    [Fact]
    public async Task Handle_WithDuplicateName_ReturnsConflict()
    {
        DivisionQueryFake query = new DivisionQueryFake().WithExistingName("Division", 7);
        CreateDivisionCommandHandler handler = CreateHandler(query: query);

        var result = await handler.Handle(new CreateDivisionCommandBuilder().Build(), CancellationToken.None);

        ResultAssertions.AssertFailedWith<ConflictError>(result);
    }

    [Fact]
    public async Task Handle_WithDuplicateName_DoesNotSaveChanges()
    {
        DivisionRepositoryFake repository = new();
        UnitOfWorkFake unitOfWork = new();
        DivisionQueryFake query = new DivisionQueryFake().WithExistingName("Division", 7);
        CreateDivisionCommandHandler handler = CreateHandler(repository, query, unitOfWork);

        await handler.Handle(new CreateDivisionCommandBuilder().Build(), CancellationToken.None);

        Assert.Equal(0, repository.AddCalls);
        Assert.Equal(0, unitOfWork.SaveChangesCalls);
    }

    [Fact]
    public async Task Handle_WithValidCommand_CallsSaveChangesOnce()
    {
        UnitOfWorkFake unitOfWork = new();
        CreateDivisionCommandHandler handler = CreateHandler(unitOfWork: unitOfWork);

        await handler.Handle(new CreateDivisionCommandBuilder().Build(), CancellationToken.None);

        Assert.Equal(1, unitOfWork.SaveChangesCalls);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ReturnsCreatedDivisionId()
    {
        DivisionRepositoryFake repository = new() { NextId = 42 };
        CreateDivisionCommandHandler handler = CreateHandler(repository);

        var result = await handler.Handle(new CreateDivisionCommandBuilder().Build(), CancellationToken.None);

        ResultAssertions.AssertSucceeded(result);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public async Task Handle_NormalizesNameBeforeCheckingDuplicate()
    {
        DivisionQueryFake query = new();
        CreateDivisionCommandHandler handler = CreateHandler(query: query);
        CreateDivisionCommand command = new CreateDivisionCommandBuilder()
            .WithName("  Division   Name  ")
            .Build();

        await handler.Handle(command, CancellationToken.None);

        Assert.Equal("Division Name", query.LastExistsByName);
    }

    private static CreateDivisionCommandHandler CreateHandler(
        DivisionRepositoryFake? repository = null,
        DivisionQueryFake? query = null,
        UnitOfWorkFake? unitOfWork = null)
        => new(
            repository ?? new DivisionRepositoryFake(),
            query ?? new DivisionQueryFake(),
            unitOfWork ?? new UnitOfWorkFake());
}
