using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductsAndPricingNew.AdminApi.Contracts.Package;
using ProductsAndPricingNew.AdminApi.Extensions;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Package.Commands.CreatePackage;
using ProductsAndPricingNew.Application.Features.Package.Commands.UpdatePackage;
using ProductsAndPricingNew.Application.Features.Package.Models;
using ProductsAndPricingNew.Application.Features.Package.Queries.GetPackageById;
using ProductsAndPricingNew.Application.Features.Package.Queries.GetPackages;

namespace ProductsAndPricingNew.AdminApi.Controllers;

/// <summary>
/// Provides endpoints for managing packages.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Produces("application/json")]
public class PackageController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public PackageController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets a paged list of packages.
    /// </summary>
    /// <param name="divisionId">Division identifier.</param>
    /// <param name="request">Filtering and paging options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A paged list of packages.</returns>
    /// <response code="200">Returns the requested page of packages.</response>
    [HttpGet("/api/v1/divisions/{divisionId:int:min(1)}/packages")]
    [ProducesResponseType(typeof(PagedResult<PackageListItemDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetList(int divisionId, [FromQuery] GetPackagesRequest request, CancellationToken ct)
    {
        GetPackagesQuery query = _mapper.Map<GetPackagesQuery>(request) with { DivisionId = divisionId };
        Result<PagedResult<PackageListItemDto>> result = await _sender.Send(query, ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Gets package by identifier.
    /// </summary>
    /// <param name="id">Package identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Package details.</returns>
    /// <response code="200">Returns the package details.</response>
    /// <response code="404">Package was not found.</response>
    [HttpGet("/api/v1/packages/{id:int:min(1)}", Name = "GetPackageById")]
    [ProducesResponseType(typeof(PackageDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetById(int id, CancellationToken ct)
    {
        Result<PackageDetailsDto> result = await _sender.Send(new GetPackageByIdQuery(id), ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Creates a package.
    /// </summary>
    /// <param name="divisionId">Division identifier.</param>
    /// <param name="request">Package creation payload.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created package identifier.</returns>
    /// <response code="201">Package was created.</response>
    /// <response code="400">Request validation failed.</response>
    /// <response code="409">Package conflicts with current state, for example duplicate name.</response>
    [HttpPost("/api/v1/divisions/{divisionId:int:min(1)}/packages")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Create(int divisionId, [FromBody] CreatePackageRequest request, CancellationToken ct)
    {
        CreatePackageCommand command = _mapper.Map<CreatePackageCommand>(request) with { DivisionId = divisionId };
        Result<int> result = await _sender.Send(command, ct);

        return result.ToActionResult(
            this,
            createdId => CreatedAtRoute("GetPackageById", new { id = createdId }, new { id = createdId }));
    }

    /// <summary>
    /// Updates an existing package.
    /// </summary>
    /// <param name="id">Package identifier.</param>
    /// <param name="request">Package update payload.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>No content when update succeeds.</returns>
    /// <response code="204">Package was updated.</response>
    /// <response code="400">Request validation failed.</response>
    /// <response code="404">Package was not found.</response>
    /// <response code="409">Package conflicts with current state, for example duplicate name or concurrency conflict.</response>
    [HttpPut("/api/v1/packages/{id:int:min(1)}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Update(int id, [FromBody] UpdatePackageRequest request, CancellationToken ct)
    {
        UpdatePackageCommand command = _mapper.Map<UpdatePackageCommand>(request) with { Id = id };
        Result<Unit> result = await _sender.Send(command, ct);
        return result.ToActionResult(this);
    }
}
