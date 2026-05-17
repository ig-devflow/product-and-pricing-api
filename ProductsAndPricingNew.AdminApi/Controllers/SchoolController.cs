using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductsAndPricingNew.AdminApi.Contracts.School;
using ProductsAndPricingNew.AdminApi.Extensions;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.School.Commands.CreateSchool;
using ProductsAndPricingNew.Application.Features.School.Commands.UpdateSchool;
using ProductsAndPricingNew.Application.Features.School.Models;
using ProductsAndPricingNew.Application.Features.School.Queries.GetSchoolById;
using ProductsAndPricingNew.Application.Features.School.Queries.GetSchools;

namespace ProductsAndPricingNew.AdminApi.Controllers;

/// <summary>
/// Provides endpoints for managing schools.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Produces("application/json")]
public class SchoolController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public SchoolController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets a paged list of schools that belong to the specified centre.
    /// </summary>
    /// <remarks>
    /// <param name="centreId"></param>
    /// <param name="request">Filtering and paging options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A paged list of schools that belong to the specified centre.</returns>
    /// <response code="200">Returns the requested page of schools that belong to the specified centre.</response>
    /// Returns an empty page if the centre has no schools. Centre existence is not enforced here —
    /// </remarks>
    /// <response code="200">Returns the requested page of schools for the centre.</response>
    [HttpGet("api/v1/centres/{centreId:int:min(1)}/schools")]
    [ProducesResponseType(typeof(PagedResult<SchoolListItemDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetListByCentre(int centreId, [FromQuery] GetSchoolsRequest request, CancellationToken ct)
    {
        var query = _mapper.Map<GetSchoolsQuery>(request) with { CentreId = centreId };
        Result<PagedResult<SchoolListItemDto>> result = await _sender.Send(query, ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Gets a paged list of schools.
    /// </summary>
    /// <param name="request">Filtering and paging options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A paged list of schools.</returns>
    /// <response code="200">Returns the requested page of schools.</response>
    [HttpGet("api/v1/schools")]
    [ProducesResponseType(typeof(PagedResult<SchoolListItemDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetList([FromQuery] GetSchoolsRequest request, CancellationToken ct)
    {
        var query = _mapper.Map<GetSchoolsQuery>(request);
        Result<PagedResult<SchoolListItemDto>> result = await _sender.Send(query, ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Gets a school by identifier.
    /// </summary>
    /// <param name="id">School identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>School details.</returns>
    /// <response code="200">Returns the centre details.</response>
    /// <response code="404">School was not found.</response>
    [HttpGet("api/v1/schools/{id:int:min(1)}", Name = "GetSchoolById")]
    [ProducesResponseType(typeof(SchoolDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetById(int id, CancellationToken ct)
    {
        Result<SchoolDetailsDto> result = await _sender.Send(new GetSchoolByIdQuery(id), ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Creates a school.
    /// </summary>
    /// <param name="centreId"></param>
    /// <param name="request">School creation payload.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created centre identifier.</returns>
    /// <response code="201">School was created.</response>
    /// <response code="400">Request validation failed.</response>
    /// <response code="409">School conflicts with current state, for example duplicate name.</response>
    [HttpPost("api/v1/centres/{centreId:int:min(1)}/schools")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Create(int centreId, [FromBody] CreateSchoolRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateSchoolCommand>(request) with { CentreId = centreId };
        Result<int> result = await _sender.Send(command, ct);

        return result.ToActionResult(
            this,
            createdId => CreatedAtRoute("GetSchoolById", new { id = createdId }, new { id = createdId }));
    }

    /// <summary>
    /// Updates an existing school.
    /// </summary>
    /// <param name="id">School identifier.</param>
    /// <param name="request">School update payload.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>No content when update succeeds.</returns>
    /// <response code="204">School was updated.</response>
    /// <response code="400">Request validation failed.</response>
    /// <response code="404">School was not found.</response>
    /// <response code="409">School conflicts with current state, for example duplicate name or concurrency conflict.</response>
    [HttpPut("api/v1/schools/{id:int:min(1)}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateSchoolRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<UpdateSchoolCommand>(request) with { Id = id };
        Result<Unit> result = await _sender.Send(command, ct);
        return result.ToActionResult(this);
    }
}