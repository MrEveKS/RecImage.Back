using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecImage.Business.Features.GetImagesList;
using RecImage.Business.Features.GetRandomImageId;

namespace RecImage.Api.Controllers;

[Route("api/[controller]/[action]")]
[Produces("application/json")]
public class ImagesController : Controller
{
    private readonly IMediator _mediator;

    public ImagesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public JsonResult GetAll(CancellationToken cancellationToken = default)
    {
        var result = _mediator.Send(new GetImagesListQuery(), cancellationToken);
        return new JsonResult(result);
    }

    [HttpPost]
    public JsonResult GetRandomId(CancellationToken cancellationToken = default)
    {
        var result = _mediator.Send(new GetRandomImageIdQuery(), cancellationToken);
        return new JsonResult(result);
    }
}