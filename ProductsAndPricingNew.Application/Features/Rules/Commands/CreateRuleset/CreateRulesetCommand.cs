using FluentResults;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Features.Rules.Models;
using ProductsAndPricingNew.Domain.Entities.Rules;

namespace ProductsAndPricingNew.Application.Features.Rules.Commands.CreateRuleset;

public sealed record CreateRulesetCommand(
    string Name,
    string? Description,
    RuleSubjectType SubjectType,
    int DivisionId,
    IReadOnlyList<RulesetRuleModel> Rules
) : ICommand<Result<int>>;
