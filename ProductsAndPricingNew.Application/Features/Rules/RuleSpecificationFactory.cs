using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using FluentResults;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Features.Rules.Models;
using ProductsAndPricingNew.Domain.Entities.Rules;
using ProductsAndPricingNew.Domain.Pricing.Specifications.Ast;
using ProductsAndPricingNew.Domain.Pricing.Specifications.Catalog;

namespace ProductsAndPricingNew.Application.Features.Rules;

/// <summary>
/// Translates a UI-authored rule tree (<see cref="RuleGroupModel"/>) into the canonical
/// specification AST (<see cref="RuleNode"/>), validating every condition against the field
/// catalog: the field must exist for the subject, the operator must be allowed for the
/// field, and the value(s) must parse to the field's data type. All validation problems are
/// collected and returned as a single <see cref="ValidationError"/>.
/// </summary>
public sealed class RuleSpecificationFactory
{
    private readonly IFieldCatalog _fieldCatalog;

    public RuleSpecificationFactory(IFieldCatalog fieldCatalog)
    {
        _fieldCatalog = fieldCatalog;
    }

    public Result<RuleNode> Build(RuleSubjectType subject, RuleGroupModel root, string rootPath = "rule")
    {
        ErrorBag errors = new();
        RuleNode node = BuildGroup(subject, root, rootPath, errors);

        return errors.HasErrors
            ? Result.Fail(new ValidationError(errors.ToDictionary()))
            : Result.Ok(node);
    }

    /// <summary>Builds the AST and serializes it to the JSON stored in <c>PricingRule.ScriptJson</c>.</summary>
    public Result<string> BuildScriptJson(RuleSubjectType subject, RuleGroupModel root, string rootPath = "rule") =>
        Build(subject, root, rootPath).Map(RuleSpecJson.Serialize);

    private RuleNode BuildGroup(RuleSubjectType subject, RuleGroupModel group, string path, ErrorBag errors)
    {
        IReadOnlyList<RuleConditionModel> conditions = group.Conditions ?? [];
        IReadOnlyList<RuleGroupModel> subGroups = group.Groups ?? [];

        if (conditions.Count == 0 && subGroups.Count == 0)
        {
            errors.Add(path, "A rule group must contain at least one condition or sub-group.");
            return new FalseNode();
        }

        List<RuleNode> children = [];

        for (int i = 0; i < conditions.Count; i++)
            children.Add(BuildCondition(subject, conditions[i], $"{path}.conditions[{i}]", errors));

        for (int i = 0; i < subGroups.Count; i++)
            children.Add(BuildGroup(subject, subGroups[i], $"{path}.groups[{i}]", errors));

        RuleNode combined = group.Logic == RuleLogic.Or ? new OrNode(children) : new AndNode(children);

        return group.Negate ? new NotNode(combined) : combined;
    }

    private RuleNode BuildCondition(RuleSubjectType subject, RuleConditionModel condition, string path, ErrorBag errors)
    {
        FieldDescriptor? field = _fieldCatalog.Find(subject, condition.Field ?? string.Empty);
        if (field is null)
        {
            errors.Add(path, $"Unknown field '{condition.Field}' for subject '{subject}'.");
            return new FalseNode();
        }

        if (!Enum.TryParse(condition.Operator, ignoreCase: true, out RuleOperator op) || !Enum.IsDefined(op))
        {
            errors.Add(path, $"Unknown operator '{condition.Operator}'.");
            return new FalseNode();
        }

        if (!field.AllowedOperators.Contains(op))
        {
            errors.Add(path, $"Operator '{op}' is not allowed for field '{field.Key}'.");
            return new FalseNode();
        }

        return op switch
        {
            RuleOperator.HasValue => new HasValueNode(field.Key),
            RuleOperator.In => BuildIn(field, condition, path, errors, negate: false),
            RuleOperator.NotIn => BuildIn(field, condition, path, errors, negate: true),
            _ => BuildComparison(field, op, condition, path, errors)
        };
    }

    private static RuleNode BuildComparison(
        FieldDescriptor field, RuleOperator op, RuleConditionModel condition, string path, ErrorBag errors)
    {
        if (condition.Value is null)
        {
            errors.Add(path, $"Operator '{op}' requires a value.");
            return new FalseNode();
        }

        return TryParseValue(field, condition.Value, path, errors, out RuleValue? value)
            ? new ComparisonNode(field.Key, ToComparisonOperator(op), value)
            : new FalseNode();
    }

    private static RuleNode BuildIn(
        FieldDescriptor field, RuleConditionModel condition, string path, ErrorBag errors, bool negate)
    {
        IReadOnlyList<string> rawValues = condition.Values ?? [];
        if (rawValues.Count == 0)
        {
            errors.Add(path, "The In / NotIn operator requires at least one value.");
            return new FalseNode();
        }

        List<RuleValue> values = [];
        foreach (string raw in rawValues)
        {
            if (TryParseValue(field, raw, path, errors, out RuleValue? value))
                values.Add(value);
        }

        if (values.Count == 0)
            return new FalseNode();

        RuleNode inNode = new InNode(field.Key, values);
        return negate ? new NotNode(inNode) : inNode;
    }

    private static bool TryParseValue(
        FieldDescriptor field, string raw, string path, ErrorBag errors, [NotNullWhen(true)] out RuleValue? value)
    {
        value = null;

        switch (field.DataType)
        {
            case RuleValueType.String:
                value = RuleValue.String(raw);
                return true;

            case RuleValueType.Int:
                if (long.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out long parsedInt))
                    value = RuleValue.Int(parsedInt);
                break;

            case RuleValueType.Decimal:
                if (decimal.TryParse(raw, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal parsedDecimal))
                    value = RuleValue.Decimal(parsedDecimal);
                break;

            case RuleValueType.Bool:
                if (bool.TryParse(raw, out bool parsedBool))
                    value = RuleValue.Bool(parsedBool);
                break;

            case RuleValueType.Date:
                if (DateOnly.TryParseExact(raw, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly parsedDate))
                    value = RuleValue.Date(parsedDate);
                break;

            case RuleValueType.Enum:
                if (field.EnumValues is not null && field.EnumValues.Contains(raw))
                    value = RuleValue.Enum(raw);
                break;
        }

        if (value is not null)
            return true;

        errors.Add(path, $"Value '{raw}' is not valid for field '{field.Key}' ({field.DataType}).");
        return false;
    }

    private static ComparisonOperator ToComparisonOperator(RuleOperator op) => op switch
    {
        RuleOperator.Eq => ComparisonOperator.Eq,
        RuleOperator.NotEq => ComparisonOperator.NotEq,
        RuleOperator.Lt => ComparisonOperator.Lt,
        RuleOperator.Lte => ComparisonOperator.Lte,
        RuleOperator.Gt => ComparisonOperator.Gt,
        RuleOperator.Gte => ComparisonOperator.Gte,
        _ => throw new ArgumentOutOfRangeException(nameof(op), op, "Not a comparison operator.")
    };

    private sealed class ErrorBag
    {
        private readonly Dictionary<string, List<string>> _errors = [];

        public bool HasErrors => _errors.Count > 0;

        public void Add(string path, string message)
        {
            if (!_errors.TryGetValue(path, out List<string>? messages))
            {
                messages = [];
                _errors[path] = messages;
            }

            messages.Add(message);
        }

        public IReadOnlyDictionary<string, string[]> ToDictionary() =>
            _errors.ToDictionary(pair => pair.Key, pair => pair.Value.ToArray());
    }
}
