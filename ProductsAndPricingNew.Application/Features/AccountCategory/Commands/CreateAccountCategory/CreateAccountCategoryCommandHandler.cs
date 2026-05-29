using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Features.AccountCategory.Abstractions;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.Repositories;
using AccountCategoryEntity = ProductsAndPricingNew.Domain.Entities.PricingRef.AccountCategory;

namespace ProductsAndPricingNew.Application.Features.AccountCategory.Commands.CreateAccountCategory;

internal sealed class CreateAccountCategoryCommandHandler : IRequestHandler<CreateAccountCategoryCommand, Result<int>>
{
    private readonly IAccountCategoryRepository _repository;
    private readonly IAccountCategoryQuery _query;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAccountCategoryCommandHandler(
        IAccountCategoryRepository repository,
        IAccountCategoryQuery query,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _query = query;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(CreateAccountCategoryCommand request, CancellationToken ct)
    {
        string name = request.Name.AsRequiredText(nameof(request.Name));

        bool isNameTaken = await _query.ExistsByNameAsync(name, request.DivisionId, ct: ct);
        if (isNameTaken)
            return Result.Fail(new ConflictError($"Account category name: '{name}' already exists in this division"));

        AccountCategoryEntity category = AccountCategoryEntity.Create(request.DivisionId, name, request.IsActive);

        await _repository.AddAsync(category, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Ok(category.Id);
    }
}
