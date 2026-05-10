using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductsAndPricingNew.AdminApi.Extensions;
using ProductsAndPricingNew.Application.Features.ReferenceData.Models;
using ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetAudiences;
using ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetContentTemplates;
using ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetCountries;
using ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetCurrencies;
using ProductsAndPricingNew.Domain.ReferenceData;

namespace ProductsAndPricingNew.AdminApi.Controllers;

/// <summary>
/// Provides read-only endpoints for reference data.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v1/reference-data")]
[Produces("application/json")]
public sealed class ReferenceDataController : ControllerBase
{
    private readonly ISender _sender;

    public ReferenceDataController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Gets active countries.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A list of active countries.</returns>
    /// <response code="200">Returns active countries.</response>
    [HttpGet("countries")]
    [ProducesResponseType(typeof(IReadOnlyCollection<CountryReferenceDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetCountries(CancellationToken ct)
    {
        Result<IReadOnlyCollection<CountryReferenceDto>> result = await _sender.Send(new GetCountriesQuery(), ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Gets active currencies.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A list of active currencies.</returns>
    /// <response code="200">Returns active currencies.</response>
    [HttpGet("currencies")]
    [ProducesResponseType(typeof(IReadOnlyCollection<CurrencyReferenceDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetCurrencies(CancellationToken ct)
    {
        Result<IReadOnlyCollection<CurrencyReferenceDto>> result = await _sender.Send(new GetCurrenciesQuery(), ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Gets active audiences.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A list of active audiences.</returns>
    /// <response code="200">Returns active audiences.</response>
    [HttpGet("audiences")]
    [ProducesResponseType(typeof(IReadOnlyCollection<AudienceReferenceDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAudiences(CancellationToken ct)
    {
        Result<IReadOnlyCollection<AudienceReferenceDto>> result = await _sender.Send(new GetAudiencesQuery(), ct);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Gets active content templates.
    /// </summary>
    /// <param name="scope">Optional content template scope filter.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A list of active content templates.</returns>
    /// <response code="200">Returns active content templates.</response>
    [HttpGet("content-templates")]
    [ProducesResponseType(typeof(IReadOnlyCollection<ContentTemplateReferenceDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetContentTemplates([FromQuery] ContentTemplateScope? scope, CancellationToken ct)
    {
        Result<IReadOnlyCollection<ContentTemplateReferenceDto>> result = await _sender.Send(new GetContentTemplatesQuery(scope), ct);
        return result.ToActionResult(this);
    }
}
