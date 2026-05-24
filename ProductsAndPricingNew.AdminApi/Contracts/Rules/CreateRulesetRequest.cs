using ProductsAndPricingNew.Application.Features.Rules.Models;
using ProductsAndPricingNew.Domain.Entities.Rules;

namespace ProductsAndPricingNew.AdminApi.Contracts.Rules;

/// <summary>Request body for creating a pricing ruleset together with its rules.</summary>
public sealed record CreateRulesetRequest(
    string Name,
    string? Description,
    RuleSubjectType SubjectType,
    int DivisionId,
    IReadOnlyList<RulesetRuleModel> Rules
);
