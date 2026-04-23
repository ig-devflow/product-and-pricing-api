using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using FluentResults;

namespace ProductsAndPricingNew.Application.Behaviors;

public sealed class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
    where TResponse : ResultBase
{
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkBehavior(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        TResponse response = await next(ct);

        if (response.IsSuccess)
            await _unitOfWork.SaveChangesAsync(ct);

        return response;
    }
}
