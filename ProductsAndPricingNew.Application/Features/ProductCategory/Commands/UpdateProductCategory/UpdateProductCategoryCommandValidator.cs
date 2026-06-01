using FluentValidation;
using ProductsAndPricingNew.Domain.Entities.PricingRef;

namespace ProductsAndPricingNew.Application.Features.ProductCategory.Commands.UpdateProductCategory;

internal sealed class UpdateProductCategoryCommandValidator : AbstractValidator<UpdateProductCategoryCommand>
{
    public UpdateProductCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Product category name is required.")
            .MaximumLength(CategoryBase.Rules.NameMaxLength)
            .WithMessage($"Product category name must not exceed {CategoryBase.Rules.NameMaxLength} characters.");

        RuleFor(x => x.Version)
            .NotEmpty()
            .WithMessage("Version is required.");
    }
}
