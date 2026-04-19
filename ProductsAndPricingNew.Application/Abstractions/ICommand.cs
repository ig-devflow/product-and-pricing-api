using MediatR;

namespace ProductsAndPricingNew.Application.Abstractions;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}