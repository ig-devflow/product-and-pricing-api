using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using ProductsAndPricingNew.Application.Behaviors;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.UnitTests.TestSupport.Assertions;

namespace ProductsAndPricingNew.UnitTests.Application.Common;

public sealed class ValidationBehaviorTests
{
    [Fact]
    public async Task WhenNoValidators_CallsNext()
    {
        ValidationBehavior<TestRequest, Result<int>> behavior = new([]);
        int nextCalls = 0;

        Result<int> result = await behavior.Handle(new TestRequest("Name", "Code"), ct =>
        {
            nextCalls++;
            return Task.FromResult(Result.Ok(42));
        }, CancellationToken.None);

        Assert.Equal(1, nextCalls);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public async Task WhenValidationPasses_CallsNext()
    {
        InlineValidator<TestRequest> validator = new();
        validator.RuleFor(x => x.Name).NotEmpty();
        ValidationBehavior<TestRequest, Result<int>> behavior = new([validator]);
        int nextCalls = 0;

        Result<int> result = await behavior.Handle(new TestRequest("Name", "Code"), ct =>
        {
            nextCalls++;
            return Task.FromResult(Result.Ok(42));
        }, CancellationToken.None);

        Assert.Equal(1, nextCalls);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public async Task WhenValidationFails_ReturnsFailedResult()
    {
        InlineValidator<TestRequest> validator = new();
        validator.RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
        ValidationBehavior<TestRequest, Result<int>> behavior = new([validator]);

        Result<int> result = await behavior.Handle(new TestRequest("", "Code"), NextShouldNotRun, CancellationToken.None);

        ResultAssertions.AssertFailedWith<ValidationError>(result);
    }

    [Fact]
    public async Task WhenValidationFails_DoesNotCallNext()
    {
        InlineValidator<TestRequest> validator = new();
        validator.RuleFor(x => x.Name).NotEmpty();
        ValidationBehavior<TestRequest, Result<int>> behavior = new([validator]);
        int nextCalls = 0;

        await behavior.Handle(new TestRequest("", "Code"), ct =>
        {
            nextCalls++;
            return Task.FromResult(Result.Ok(42));
        }, CancellationToken.None);

        Assert.Equal(0, nextCalls);
    }

    [Fact]
    public async Task WhenValidatorThrowsDomainException_ReturnsFailedResult()
    {
        ValidationBehavior<TestRequest, Result<int>> behavior = new([new ThrowingValidator()]);

        Result<int> result = await behavior.Handle(new TestRequest("Name", "Code"), NextShouldNotRun, CancellationToken.None);

        DomainRuleViolationError error = ResultAssertions.AssertFailedWith<DomainRuleViolationError>(result);
        Assert.Equal("Broken domain rule.", error.Message);
    }

    [Fact]
    public async Task ErrorsAreMappedToExpectedValidationErrorShape()
    {
        InlineValidator<TestRequest> validator = new();
        validator.RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
        validator.RuleFor(x => x.Code).NotEmpty().WithMessage("Code is required.");
        ValidationBehavior<TestRequest, Result<int>> behavior = new([validator]);

        Result<int> result = await behavior.Handle(new TestRequest("", ""), NextShouldNotRun, CancellationToken.None);

        ValidationError error = ResultAssertions.AssertFailedWith<ValidationError>(result);
        Assert.Equal("One or more validation errors occurred.", error.Message);
        Assert.Equal(new[] { "Name is required." }, error.Errors["Name"]);
        Assert.Equal(new[] { "Code is required." }, error.Errors["Code"]);
    }

    private static Task<Result<int>> NextShouldNotRun(CancellationToken ct)
        => throw new InvalidOperationException("Next should not be called.");

    private sealed record TestRequest(string Name, string Code) : IRequest<Result<int>>;

    private sealed class ThrowingValidator : AbstractValidator<TestRequest>
    {
        public override Task<ValidationResult> ValidateAsync(ValidationContext<TestRequest> context, CancellationToken cancellation = default)
            => throw new DomainException("Broken domain rule.");
    }
}
