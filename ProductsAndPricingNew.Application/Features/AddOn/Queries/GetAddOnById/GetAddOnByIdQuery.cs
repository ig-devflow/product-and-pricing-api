using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.AddOn.Models;

namespace ProductsAndPricingNew.Application.Features.AddOn.Queries.GetAddOnById;

public sealed record GetAddOnByIdQuery(int Id) : IRequest<Result<AddOnDetailsDto>>;
