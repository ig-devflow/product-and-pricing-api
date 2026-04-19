using MediatR;
using ProductsAndPricingNew.Application.Features.Division.Dtos;

namespace ProductsAndPricingNew.Application.Features.Division.Queries.GetDivisionById;

public sealed record GetDivisionByIdQuery(int Id) : IRequest<DivisionDetailsDto?>;