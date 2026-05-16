using FluentValidation;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Application.Common.Validation.Validators;
using ProductsAndPricingNew.Application.Features.Centre.Models;
using ProductsAndPricingNew.Domain.Entities.PricingRef;

namespace ProductsAndPricingNew.Application.Features.Centre.Validation;

internal sealed class CentreBankDetailsDtoValidator : AbstractValidator<CentreBankDetailsDto>
{
    public CentreBankDetailsDtoValidator(IReferenceDataValidationQuery referenceData)
    {
        RuleFor(x => x.BeneficiaryName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Beneficiary name is required.")
            .MaximumLength(CentreBankDetails.Rules.MaxLength)
            .WithMessage($"Beneficiary name must not exceed {CentreBankDetails.Rules.MaxLength} characters.");

        RuleFor(x => x.AccountNumber)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Account number is required.")
            .MaximumLength(CentreBankDetails.Rules.MaxLength)
            .WithMessage($"Account number must not exceed {CentreBankDetails.Rules.MaxLength} characters.");

        RuleFor(x => x.BankName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Bank name is required.")
            .MaximumLength(CentreBankDetails.Rules.MaxLength)
            .WithMessage($"Bank name must not exceed {CentreBankDetails.Rules.MaxLength} characters.");

        RuleFor(x => x.Iban)
            .MaximumLength(BankIdentifiers.Rules.MaxLength)
            .WithMessage($"IBAN must not exceed {BankIdentifiers.Rules.MaxLength} characters.");

        RuleFor(x => x.SwiftCode)
            .MaximumLength(BankIdentifiers.Rules.MaxLength)
            .WithMessage($"SWIFT code must not exceed {BankIdentifiers.Rules.MaxLength} characters.");

        RuleFor(x => x.BranchCode)
            .MaximumLength(BankIdentifiers.Rules.MaxLength)
            .WithMessage($"Branch code must not exceed {BankIdentifiers.Rules.MaxLength} characters.");

        RuleFor(x => x.AbaRoutingNo)
            .MaximumLength(BankIdentifiers.Rules.MaxLength)
            .WithMessage($"ABA routing number must not exceed {BankIdentifiers.Rules.MaxLength} characters.");

        RuleFor(x => x.AchAba)
            .MaximumLength(BankIdentifiers.Rules.MaxLength)
            .WithMessage($"ACH/ABA must not exceed {BankIdentifiers.Rules.MaxLength} characters.");

        RuleFor(x => x.BankAddress)
            .SetValidator(new AddressDtoValidator(referenceData));

        RuleFor(x => x.BeneficiaryBankAddress)
            .SetValidator(new AddressDtoValidator(referenceData));

        RuleFor(x => x.IntermediaryBankAddress)
            .SetValidator(new AddressDtoValidator(referenceData));

        RuleFor(x => x.IntermediaryBankName)
            .MaximumLength(IntermediaryBank.Rules.MaxLength)
            .WithMessage($"Intermediary bank name must not exceed {IntermediaryBank.Rules.MaxLength} characters.");

        RuleFor(x => x.IntermediarySwiftCode)
            .MaximumLength(IntermediaryBank.Rules.MaxLength)
            .WithMessage($"Intermediary SWIFT code must not exceed {IntermediaryBank.Rules.MaxLength} characters.");
    }
}
