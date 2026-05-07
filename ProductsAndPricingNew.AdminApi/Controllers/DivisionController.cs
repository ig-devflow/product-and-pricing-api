using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductsAndPricingNew.AdminApi.Contracts.Division;
using ProductsAndPricingNew.AdminApi.Extensions;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Division.Commands.CreateDivision;
using ProductsAndPricingNew.Application.Features.Division.Commands.UpdateDivision;
using ProductsAndPricingNew.Application.Features.Division.Models;
using ProductsAndPricingNew.Application.Features.Division.Queries.GetDivisionById;
using ProductsAndPricingNew.Application.Features.Division.Queries.GetDivisions;

namespace ProductsAndPricingNew.AdminApi.Controllers;

[ApiController]
[Route("api/divisions")]
public sealed class DivisionController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public DivisionController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<DivisionListItemDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetList([FromQuery] GetDivisionsRequest request, CancellationToken ct)
    {
        var query = _mapper.Map<GetDivisionsQuery>(request);
        Result<PagedResult<DivisionListItemDto>> result = await _sender.Send(query, ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(DivisionDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetById(int id, CancellationToken ct)
    {
        Result<DivisionDetailsDto> result = await _sender.Send(new GetDivisionByIdQuery(id), ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Create([FromBody] CreateDivisionRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateDivisionCommand>(request);
        Result<int> result = await _sender.Send(command, ct);

        return result.ToActionResult(
            this,
            createdId => CreatedAtAction(nameof(GetById), new { id = createdId }, new { id = createdId }));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateDivisionRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<UpdateDivisionCommand>(request) with { Id = id };
        Result<Unit> result = await _sender.Send(command, ct);
        return result.ToActionResult(this);
    }
}