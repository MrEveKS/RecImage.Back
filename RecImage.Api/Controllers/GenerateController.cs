using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecImage.Business.Features.ConvertToPoints;
using RecImage.Business.Features.ConvertToPointsById;

namespace RecImage.Api.Controllers;

[Route("api/[controller]/[action]")]
[Produces("application/json")]
public class GenerateController : Controller
{
    private readonly IMediator _mediator;

    public GenerateController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<JsonResult> ConvertToPoints([FromForm] ConvertToPointsQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return new JsonResult(result);
    }

    [HttpPost]
    public async Task<JsonResult> ConvertToPointsById([FromBody] ConvertToPointsByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return new JsonResult(result);
    }
}