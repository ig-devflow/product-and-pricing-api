using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Features.Rules.Models;
using ProductsAndPricingNew.Domain.Entities.Rules;
using ProductsAndPricingNew.Domain.Repositories;

namespace ProductsAndPricingNew.Application.Features.Rules.Commands.CreateRuleset;

internal sealed class CreateRulesetCommandHandler : IRequestHandler<CreateRulesetCommand, Result<int>>
{
    private readonly IRulesetRepository _rulesetRepository;
    private readonly RuleSpecificationFactory _specificationFactory;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRulesetCommandHandler(
        IRulesetRepository rulesetRepository,
        RuleSpecificationFactory specificationFactory,
        IUnitOfWork unitOfWork)
    {
        _rulesetRepository = rulesetRepository;
        _specificationFactory = specificationFactory;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(CreateRulesetCommand request, CancellationToken ct)
    {
        // Translate and validate every rule definition against the field catalog before
        // touching the aggregate, so nothing is persisted when any rule is invalid.
        Dictionary<string, string[]> errors = [];
        List<(RulesetRuleModel Rule, string ScriptJson)> compiledRules = [];

        for (int i = 0; i < request.Rules.Count; i++)
        {
            RulesetRuleModel rule = request.Rules[i];
            Result<string> scriptJson = _specificationFactory.BuildScriptJson(
                request.SubjectType, rule.Definition, $"rules[{i}]");

            if (scriptJson.IsSuccess)
            {
                compiledRules.Add((rule, scriptJson.Value));
                continue;
            }

            foreach (ValidationError validationError in scriptJson.Errors.OfType<ValidationError>())
            {
                foreach (KeyValuePair<string, string[]> entry in validationError.Errors)
                    errors[entry.Key] = entry.Value;
            }
        }

        if (errors.Count > 0)
            return Result.Fail(new ValidationError(errors));

        Ruleset ruleset = Ruleset.Create(request.Name, request.SubjectType, request.DivisionId);
        ruleset.ChangeDescription(request.Description);

        foreach ((RulesetRuleModel rule, string scriptJson) in compiledRules)
            ruleset.AddRule(rule.Name, scriptJson, rule.Priority);

        await _rulesetRepository.AddAsync(ruleset, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Ok(ruleset.Id);
    }
}
