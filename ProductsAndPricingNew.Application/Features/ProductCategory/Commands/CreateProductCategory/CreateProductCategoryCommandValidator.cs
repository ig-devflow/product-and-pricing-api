using FluentValidation;
using ProductsAndPricingNew.Domain.Entities.PricingRef;

namespace ProductsAndPricingNew.Application.Features.ProductCategory.Commands.CreateProductCategory;

internal sealed class CreateProductCategoryCommandValidator : AbstractValidator<CreateProductCategoryCommand>
{
    public CreateProductCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Product category name is required.")
            .MaximumLength(CategoryBase.Rules.NameMaxLength)
            .WithMessage($"Product category name must not exceed {CategoryBase.Rules.NameMaxLength} characters.");
    }
}
