using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ProductsAndPricingNew.AdminApi.Controllers;

[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v1/centres")]
[Produces("application/json")]
public class CentreController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public CentreController(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }
}