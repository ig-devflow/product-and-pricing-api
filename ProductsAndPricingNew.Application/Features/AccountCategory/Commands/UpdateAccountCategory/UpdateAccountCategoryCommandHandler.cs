using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Features.AccountCategory.Abstractions;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.Repositories;
using AccountCategoryEntity = ProductsAndPricingNew.Domain.Entities.PricingRef.AccountCategory;

namespace ProductsAndPricingNew.Application.Features.AccountCategory.Commands.UpdateAccountCategory;

internal sealed class UpdateAccountCategoryCommandHandler : IRequestHandler<UpdateAccountCategoryCommand, Result<Unit>>
{
    private readonly IAccountCategoryRepository _repository;
    private readonly IAccountCategoryQuery _query;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAccountCategoryCommandHandler(
        IAccountCategoryRepository repository,
        IAccountCategoryQuery query,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _query = query;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(UpdateAccountCategoryCommand request, CancellationToken ct)
    {
        AccountCategoryEntity? category = await _repository.GetByIdAsync(request.Id, ct);
        if (category is null)
            return Result.Fail(new NotFoundError($"Account category with id {request.Id} was not found"));

        if (!category.HasVersion(request.Version))
            return Result.Fail(new ConflictError("Account category was modified by another user. Reload it and try again."));

        string name = request.Name.AsRequiredText(nameof(request.Name));

        bool isNameTaken = await _query.ExistsByNameAsync(name, category.DivisionId, request.Id, ct);
        if (isNameTaken)
            return Result.Fail(new ConflictError($"Account category name: '{name}' already exists in this division"));

        category.Rename(name);

        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Ok();
    }
}
