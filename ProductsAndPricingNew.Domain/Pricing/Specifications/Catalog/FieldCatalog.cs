using ProductsAndPricingNew.Domain.Entities.Products;
using ProductsAndPricingNew.Domain.Entities.Rules;
using ProductsAndPricingNew.Domain.Pricing.Specifications.Ast;

namespace ProductsAndPricingNew.Domain.Pricing.Specifications.Catalog;

/// <summary>
/// Code-defined field catalog. The set of fields is defined here (not as pure data)
/// because each field is also paired with an engine-side populator that extracts its value
/// from domain objects into a <see cref="RuleFact"/>.
/// </summary>
public sealed class FieldCatalog : IFieldCatalog
{
    private readonly IReadOnlyDictionary<RuleSubjectType, IReadOnlyList<FieldDescriptor>> _bySubject;
    private readonly IReadOnlyDictionary<(RuleSubjectType Subject, string Key), FieldDescriptor> _byKey;

    public FieldCatalog()
    {
        FieldDescriptor[] all =
        [
            .. ProductRowFields(),
            .. BookingFields(),
            .. CancellationFields()
        ];

        _bySubject = all
            .GroupBy(field => field.Subject)
            .ToDictionary(group => group.Key, group => (IReadOnlyList<FieldDescriptor>)group.ToList());

        _byKey = all.ToDictionary(field => (field.Subject, field.Key));
    }

    public IReadOnlyList<FieldDescriptor> GetFields(RuleSubjectType subject) => _bySubject.TryGetValue(subject, out IReadOnlyList<FieldDescriptor>? fields) ? fields : [];

    public FieldDescriptor? Find(RuleSubjectType subject, string key) =>
        _byKey.GetValueOrDefault((subject, key));

    private static IEnumerable<FieldDescriptor> ProductRowFields()
    {
        const RuleSubjectType subject = RuleSubjectType.ProductRow;
        return
        [
            Date(subject, "booking.dateBooked", "Date booked"),
            Enum<ProductKind>(subject, "product.kind", "Product type"),
            Int(subject, "product.id", "Product", FieldValueSource.Products),
            Int(subject, "line.schoolId", "School", FieldValueSource.Schools),
            Int(subject, "line.divisionId", "Division", FieldValueSource.Divisions),
            Int(subject, "line.agentId", "Agent", FieldValueSource.Agents),
            Str(subject, "line.saleCountry", "Sale country", FieldValueSource.Countries),
            Int(subject, "line.weeks", "Number of weeks"),
            Int(subject, "student.age", "Student age on arrival")
        ];
    }

    private static IEnumerable<FieldDescriptor> BookingFields()
    {
        const RuleSubjectType subject = RuleSubjectType.Booking;
        return
        [
            Date(subject, "booking.dateBooked", "Date booked"),
            Int(subject, "booking.divisionId", "Division", FieldValueSource.Divisions),
            Int(subject, "booking.agentId", "Agent", FieldValueSource.Agents),
            Str(subject, "booking.saleCountry", "Sale country", FieldValueSource.Countries)
        ];
    }

    private static IEnumerable<FieldDescriptor> CancellationFields()
    {
        const RuleSubjectType subject = RuleSubjectType.Cancellation;
        return
        [
            Date(subject, "cancellation.date", "Cancellation date"),
            Date(subject, "booking.dateBooked", "Date booked"),
            Int(subject, "cancellation.daysBeforeArrival", "Days before arrival")
        ];
    }

    private static FieldDescriptor Int(RuleSubjectType subject, string key, string label, FieldValueSource? source = null) =>
        new(subject, key, RuleValueType.Int, label, Operators.Numeric, source);

    private static FieldDescriptor Date(RuleSubjectType subject, string key, string label) =>
        new(subject, key, RuleValueType.Date, label, Operators.Ordered);

    private static FieldDescriptor Str(RuleSubjectType subject, string key, string label, FieldValueSource? source = null) =>
        new(subject, key, RuleValueType.String, label, Operators.Text, source);

    private static FieldDescriptor Enum<TEnum>(RuleSubjectType subject, string key, string label)
        where TEnum : struct, Enum =>
        new(subject, key, RuleValueType.Enum, label, Operators.Text, ValueSource: null, EnumValues: System.Enum.GetNames<TEnum>());

    private static class Operators
    {
        public static readonly IReadOnlyList<RuleOperator> Numeric =
        [
            RuleOperator.Eq, RuleOperator.NotEq, RuleOperator.Lt, RuleOperator.Lte,
            RuleOperator.Gt, RuleOperator.Gte, RuleOperator.In, RuleOperator.NotIn, RuleOperator.HasValue
        ];

        public static readonly IReadOnlyList<RuleOperator> Ordered =
        [
            RuleOperator.Eq, RuleOperator.NotEq, RuleOperator.Lt, RuleOperator.Lte,
            RuleOperator.Gt, RuleOperator.Gte, RuleOperator.HasValue
        ];

        public static readonly IReadOnlyList<RuleOperator> Text =
        [
            RuleOperator.Eq, RuleOperator.NotEq, RuleOperator.In, RuleOperator.NotIn, RuleOperator.HasValue
        ];
    }
}
