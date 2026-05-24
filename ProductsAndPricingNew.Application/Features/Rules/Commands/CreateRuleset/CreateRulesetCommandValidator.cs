using FluentValidation;
using ProductsAndPricingNew.Application.Features.Rules.Models;
using ProductsAndPricingNew.Domain.Entities.Rules;

namespace ProductsAndPricingNew.Application.Features.Rules.Commands.CreateRuleset;

internal sealed class CreateRulesetCommandValidator : AbstractValidator<CreateRulesetCommand>
{
    public CreateRulesetCommandValidator()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Ruleset name is required.")
            .MaximumLength(Ruleset.EntityRules.NameMaxLength)
            .WithMessage($"Ruleset name must not exceed {Ruleset.EntityRules.NameMaxLength} characters.");

        RuleFor(x => x.Description)
            .MaximumLength(Ruleset.EntityRules.DescriptionMaxLength)
            .WithMessage($"Description must not exceed {Ruleset.EntityRules.DescriptionMaxLength} characters.");

        RuleFor(x => x.SubjectType)
            .IsInEnum().WithMessage("Unknown rule subject type.");

        RuleFor(x => x.DivisionId)
            .GreaterThan(0).WithMessage("Division is required.");

        RuleFor(x => x.Rules)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Rules collection is required.")
            .Must(HaveUniqueNames).WithMessage("Rule names must be unique within a ruleset.");

        RuleForEach(x => x.Rules).ChildRules(rule =>
        {
            rule.RuleFor(r => r.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Rule name is required.")
                .MaximumLength(PricingRule.Rules.NameMaxLength)
                .WithMessage($"Rule name must not exceed {PricingRule.Rules.NameMaxLength} characters.");

            rule.RuleFor(r => r.Priority)
                .GreaterThanOrEqualTo(0).WithMessage("Rule priority cannot be negative.");

            rule.RuleFor(r => r.Definition)
                .NotNull().WithMessage("Rule definition is required.");
        });
    }

    private static bool HaveUniqueNames(IReadOnlyList<RulesetRuleModel>? rules)
    {
        if (rules is null)
            return true;

        HashSet<string> seen = new(StringComparer.OrdinalIgnoreCase);
        foreach (RulesetRuleModel rule in rules)
        {
            if (string.IsNullOrWhiteSpace(rule.Name))
                continue;

            if (!seen.Add(rule.Name.Trim()))
                return false;
        }

        return true;
    }
}
