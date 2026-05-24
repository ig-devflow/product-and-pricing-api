using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductsAndPricingNew.AdminApi.Contracts.Rules;
using ProductsAndPricingNew.AdminApi.Extensions;
using ProductsAndPricingNew.Application.Features.Pricing.Models;
using ProductsAndPricingNew.Application.Features.Pricing.Queries.GetFieldCatalog;
using ProductsAndPricingNew.Application.Features.Rules.Commands.CreateRuleset;
using ProductsAndPricingNew.Domain.Entities.Rules;

namespace ProductsAndPricingNew.AdminApi.Controllers;

/// <summary>
/// Endpoints supporting the pricing rule-builder.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v1/pricing")]
[Produces("application/json")]
public sealed class PricingController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public PricingController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets the fields available to rules of the given subject.
    /// </summary>
    /// <param name="subject">The rule subject (e.g. ProductRow, Booking, Cancellation).</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The fields a rule of that subject may reference.</returns>
    /// <response code="200">Returns the field catalog for the subject.</response>
    [HttpGet("field-catalog")]
    [ProducesResponseType(typeof(IReadOnlyCollection<FieldDescriptorDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetFieldCatalog([FromQuery] RuleSubjectType subject, CancellationToken ct)
    {
        Result<IReadOnlyCollection<FieldDescriptorDto>> result = await _sender.Send(new GetFieldCatalogQuery(subject), ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Creates a pricing ruleset together with its rules.
    /// </summary>
    /// <param name="request">The ruleset creation payload.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>The created ruleset identifier.</returns>
    /// <response code="201">The ruleset was created.</response>
    /// <response code="400">Request or rule validation failed.</response>
    [HttpPost("rulesets")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateRuleset([FromBody] CreateRulesetRequest request, CancellationToken ct)
    {
        CreateRulesetCommand command = _mapper.Map<CreateRulesetCommand>(request);
        Result<int> result = await _sender.Send(command, ct);

        return result.ToActionResult(
            this,
            createdId => Created($"/api/v1/pricing/rulesets/{createdId}", new { id = createdId }));
    }
}
