using FluentValidation;
using ProductsAndPricingNew.Domain.Entities.PricingRef;

namespace ProductsAndPricingNew.Application.Features.AccountCategory.Commands.UpdateAccountCategory;

internal sealed class UpdateAccountCategoryCommandValidator : AbstractValidator<UpdateAccountCategoryCommand>
{
    public UpdateAccountCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Account category name is required.")
            .MaximumLength(CategoryBase.Rules.NameMaxLength)
            .WithMessage($"Account category name must not exceed {CategoryBase.Rules.NameMaxLength} characters.");

        RuleFor(x => x.Version)
            .NotEmpty()
            .WithMessage("Version is required.");
    }
}
