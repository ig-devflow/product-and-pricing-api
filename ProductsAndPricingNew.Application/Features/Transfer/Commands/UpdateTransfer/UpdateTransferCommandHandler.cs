using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Features.Transfer.Abstractions;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.Repositories;
using ProductsAndPricingNew.Domain.UnitOfMeasure;
using TransferEntity = ProductsAndPricingNew.Domain.Entities.Products.Transfer;

namespace ProductsAndPricingNew.Application.Features.Transfer.Commands.UpdateTransfer;

internal sealed class UpdateTransferCommandHandler : IRequestHandler<UpdateTransferCommand, Result<Unit>>
{
    private readonly ITransferQuery _transferQuery;
    private readonly ITransferRepository _transferRepository;
    private readonly IUnitTypeProvider _unitTypeProvider;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTransferCommandHandler(
        ITransferQuery transferQuery,
        ITransferRepository transferRepository,
        IUnitTypeProvider unitTypeProvider,
        IUnitOfWork unitOfWork)
    {
        _transferQuery = transferQuery;
        _transferRepository = transferRepository;
        _unitTypeProvider = unitTypeProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(UpdateTransferCommand request, CancellationToken ct)
    {
        TransferEntity? course = await _transferRepository.GetByIdAsync(request.Id, ct);
        if (course is null)
            return Result.Fail(new NotFoundError($"Transfer with id {request.Id} was not found"));

        if (!course.HasVersion(request.Version))
            return Result.Fail(new ConflictError("Transfer was modified by another user. Reload it and try again."));

        string name = request.Name.AsRequiredText(nameof(request.Name));

        bool isNameTaken = await _transferQuery.ExistsByNameAsync(name, request.Id, ct);
        if (isNameTaken)
            return Result.Fail(new ConflictError($"Transfer name: '{name}' already exists"));

        UnitType unitType = _unitTypeProvider.Get(request.UnitTypeId);

        course.Rename(name);
        course.SetIsActive(request.IsActive);
        course.WithTransferType(request.TransferTypeId);
        course.WithTransferPort(request.TransferPortId);
        course.WithUnitType(unitType);
        course.WithTimeWindow(request.TimeFrom, request.TimeTo);
        course.WithCategories(request.AccountCategoryId, request.ProductCategoryId);
        course.WithFinanceCodes(request.GeneralLedgerCode, request.CostCentreCode);
        course.WithClosurePolicy(request.ClosurePolicy);

        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Ok();
    }
}