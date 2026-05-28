using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductsAndPricingNew.AdminApi.Contracts.Course;
using ProductsAndPricingNew.AdminApi.Extensions;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Course.Commands.CreateCourse;
using ProductsAndPricingNew.Application.Features.Course.Commands.UpdateCourse;
using ProductsAndPricingNew.Application.Features.Course.Models;
using ProductsAndPricingNew.Application.Features.Course.Queries.GetCourseById;
using ProductsAndPricingNew.Application.Features.Course.Queries.GetCourses;

namespace ProductsAndPricingNew.AdminApi.Controllers;

/// <summary>
/// Provides endpoints for managing courses.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Produces("application/json")]
public class CourseController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public CourseController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets a paged list of courses.
    /// </summary>
    /// <param name="divisionId">Division identifier.</param>
    /// <param name="request">Filtering and paging options.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A paged list of courses.</returns>
    /// <response code="200">Returns the requested page of courses.</response>
    [HttpGet("/api/v1/divisions/{divisionId:int:min(1)}/courses")]
    [ProducesResponseType(typeof(PagedResult<CourseListItemDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetList(int divisionId, [FromQuery] GetCoursesRequest request, CancellationToken ct)
    {
        GetCoursesQuery query = _mapper.Map<GetCoursesQuery>(request) with { DivisionId = divisionId };
        Result<PagedResult<CourseListItemDto>> result = await _sender.Send(query, ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Gets course by identifier.
    /// </summary>
    /// <param name="id">Course identifier.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Course details.</returns>
    /// <response code="200">Returns the course details.</response>
    /// <response code="404">Course was not found.</response>
    [HttpGet("/api/v1/courses/{id:int:min(1)}", Name = "GetCourseById")]
    [ProducesResponseType(typeof(CourseDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetById(int id, CancellationToken ct)
    {
        Result<CourseDetailsDto> result = await _sender.Send(new GetCourseByIdQuery(id), ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Creates a course.
    /// </summary>
    /// <param name="divisionId">Division identifier.</param>
    /// <param name="request">Course creation payload.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created course identifier.</returns>
    /// <response code="201">Course was created.</response>
    /// <response code="400">Request validation failed.</response>
    /// <response code="409">Course conflicts with current state, for example duplicate name.</response>
    [HttpPost("/api/v1/divisions/{divisionId:int:min(1)}/courses")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Create(int divisionId, [FromBody] CreateCourseRequest request, CancellationToken ct)
    {
        CreateCourseCommand command = _mapper.Map<CreateCourseCommand>(request) with { DivisionId = divisionId };
        Result<int> result = await _sender.Send(command, ct);

        return result.ToActionResult(
            this,
            createdId => CreatedAtRoute("GetCourseById", new { id = createdId }, new { id = createdId }));
    }

    /// <summary>
    /// Updates an existing course.
    /// </summary>
    /// <param name="id">Course identifier.</param>
    /// <param name="request">Course update payload.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>No content when update succeeds.</returns>
    /// <response code="204">Course was updated.</response>
    /// <response code="400">Request validation failed.</response>
    /// <response code="404">Course was not found.</response>
    /// <response code="409">Course conflicts with current state, for example duplicate name or concurrency conflict.</response>
    [HttpPut("/api/v1/courses/{id:int:min(1)}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateCourseRequest request, CancellationToken ct)
    {
        UpdateCourseCommand command = _mapper.Map<UpdateCourseCommand>(request) with { Id = id };
        Result<Unit> result = await _sender.Send(command, ct);
        return result.ToActionResult(this);
    }
}
