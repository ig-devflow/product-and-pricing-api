using FluentValidation;
using ProductsAndPricingNew.Domain.Entities.PricingRef;

namespace ProductsAndPricingNew.Application.Features.AccountCategory.Commands.CreateAccountCategory;

internal sealed class CreateAccountCategoryCommandValidator : AbstractValidator<CreateAccountCategoryCommand>
{
    public CreateAccountCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Account category name is required.")
            .MaximumLength(CategoryBase.Rules.NameMaxLength)
            .WithMessage($"Account category name must not exceed {CategoryBase.Rules.NameMaxLength} characters.");
    }
}
