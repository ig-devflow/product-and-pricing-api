using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Features.Transfer.Abstractions;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.Repositories;
using ProductsAndPricingNew.Domain.UnitOfMeasure;
using TransferEntity = ProductsAndPricingNew.Domain.Entities.Products.Transfer;

namespace ProductsAndPricingNew.Application.Features.Transfer.Commands.CreateTransfer;

internal sealed class CreateTransferCommandHandler : IRequestHandler<CreateTransferCommand, Result<int>>
{
    private readonly ITransferQuery _transferQuery;
    private readonly ITransferRepository _transferRepository;
    private readonly IUnitTypeProvider _unitTypeProvider;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTransferCommandHandler(
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

    public async Task<Result<int>> Handle(CreateTransferCommand request, CancellationToken ct)
    {
        string name = request.Name.AsRequiredText(nameof(request.Name));

        bool isNameTaken = await _transferQuery.ExistsByNameAsync(name, ct: ct);
        if (isNameTaken)
            return Result.Fail(new ConflictError($"Transfer name: '{name}' already exists"));

        UnitType unitType = _unitTypeProvider.Get(request.UnitTypeId);

        TransferEntity transfer = new TransferEntity.Builder(request.DivisionId, name, request.TransferTypeId, request.TransferPortId, unitType)
            .SetIsActive(request.IsActive)
            .WithTimeWindow(request.TimeFrom, request.TimeTo)
            .WithCategories(request.AccountCategoryId, request.ProductCategoryId)
            .WithFinanceCodes(request.GeneralLedgerCode, request.CostCentreCode)
            .WithClosurePolicy(request.ClosurePolicy)
            .Build();

        await _transferRepository.AddAsync(transfer, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Ok(transfer.Id);
    }
}