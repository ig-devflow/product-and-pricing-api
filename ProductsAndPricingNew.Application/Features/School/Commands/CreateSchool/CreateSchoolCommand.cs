using FluentResults;
using ProductsAndPricingNew.Application.Abstractions;

namespace ProductsAndPricingNew.Application.Features.School.Commands.CreateSchool;

public record CreateSchoolCommand() : ICommand<Result<int>>;