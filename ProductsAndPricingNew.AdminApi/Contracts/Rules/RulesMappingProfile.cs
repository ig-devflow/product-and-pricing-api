using AutoMapper;
using ProductsAndPricingNew.Application.Features.Rules.Commands.CreateRuleset;

namespace ProductsAndPricingNew.AdminApi.Contracts.Rules;

public sealed class RulesMappingProfile : Profile
{
    public RulesMappingProfile()
    {
        CreateMap<CreateRulesetRequest, CreateRulesetCommand>()
            .ConstructUsing(src => new CreateRulesetCommand(
                src.Name,
                src.Description,
                src.SubjectType,
                src.DivisionId,
                src.Rules));
    }
}
