using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.Division.Models;

namespace ProductsAndPricingNew.Application.Features.Division.Queries.GetDivisionById;

public sealed record GetDivisionByIdQuery(int Id) : IRequest<Result<DivisionDetailsDto>>;