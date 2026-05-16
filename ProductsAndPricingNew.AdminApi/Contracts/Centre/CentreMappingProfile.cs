using AutoMapper;
using ProductsAndPricingNew.Application.Common.Models;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Centre.Commands.CreateCentre;
using ProductsAndPricingNew.Application.Features.Centre.Commands.UpdateCentre;
using ProductsAndPricingNew.Application.Features.Centre.Models;
using ProductsAndPricingNew.Application.Features.Centre.Queries.GetCentres;

namespace ProductsAndPricingNew.AdminApi.Contracts.Centre;

public sealed class CentreMappingProfile : Profile
{
    public CentreMappingProfile()
    {
        CreateMap<CentreContactInfoRequest, CentreContactInfoDto>()
            .ConstructUsing((src, ctx) => new CentreContactInfoDto(
                src.GeneralEmail,
                src.AccommodationEmail,
                src.Telephone,
                src.EmergencyTelephone,
                src.TransferEmergencyTelephone,
                src.BrandColor,
                ctx.Mapper.Map<AddressDto?>(src.ContactAddress),
                ctx.Mapper.Map<ImageFileDto?>(src.LogoImage)));

        CreateMap<CentreLegalInfoRequest, CentreLegalInfoDto>();

        CreateMap<CentreOperationalRatiosRequest, CentreOperationalRatiosDto>();

        CreateMap<CentreBankDetailsRequest, CentreBankDetailsDto>()
            .ConstructUsing((src, ctx) => new CentreBankDetailsDto(
                src.BeneficiaryName,
                src.AccountNumber,
                src.BankName,
                src.Iban,
                src.SwiftCode,
                src.BranchCode,
                src.AbaRoutingNo,
                src.AchAba,
                ctx.Mapper.Map<AddressDto>(src.BankAddress),
                ctx.Mapper.Map<AddressDto>(src.BeneficiaryBankAddress),
                ctx.Mapper.Map<AddressDto>(src.IntermediaryBankAddress),
                src.IntermediaryBankName,
                src.IntermediarySwiftCode));

        CreateMap<CentreContactRequest, CentreContactDto>()
            .ConstructUsing((src, ctx) => new CentreContactDto(
                src.ContactType,
                src.Name,
                src.Email,
                ctx.Mapper.Map<ImageFileDto>(src.SignatureImage)));

        CreateMap<GetCentresRequest, GetCentresQuery>()
            .ConstructUsing(src => new GetCentresQuery(
                src.Search,
                src.IsActive,
                new PagingFilter(src.Page, src.PageSize)));

        CreateMap<CreateCentreRequest, CreateCentreCommand>()
            .ConstructUsing((src, ctx) => new CreateCentreCommand(
                src.Name,
                src.Code,
                src.CurrencyId,
                src.PrintFormat,
                src.IsActive,
                src.IsPhysicalCentre,
                ctx.Mapper.Map<CentreContactInfoDto>(src.ContactInfo),
                ctx.Mapper.Map<CentreLegalInfoDto>(src.LegalInfo),
                ctx.Mapper.Map<CentreOperationalRatiosDto>(src.OperationalRatios),
                ctx.Mapper.Map<CentreBankDetailsDto>(src.BankDetails),
                ctx.Mapper.Map<IReadOnlyCollection<CentreContactDto>>(src.Contacts),
                ctx.Mapper.Map<IReadOnlyCollection<TextContentDto>>(src.Texts)));

        CreateMap<UpdateCentreRequest, UpdateCentreCommand>()
            .ConstructUsing((src, ctx) => new UpdateCentreCommand(
                src.Id,
                src.Name,
                src.Code,
                src.CurrencyId,
                src.PrintFormat,
                src.IsActive,
                src.IsPhysicalCentre,
                ctx.Mapper.Map<CentreContactInfoDto>(src.ContactInfo),
                ctx.Mapper.Map<CentreLegalInfoDto>(src.LegalInfo),
                ctx.Mapper.Map<CentreOperationalRatiosDto>(src.OperationalRatios),
                ctx.Mapper.Map<CentreBankDetailsDto>(src.BankDetails),
                ctx.Mapper.Map<IReadOnlyCollection<CentreContactDto>>(src.Contacts),
                ctx.Mapper.Map<IReadOnlyCollection<TextContentDto>>(src.Texts),
                src.Version));
    }
}