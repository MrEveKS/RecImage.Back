using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecImage.Business.Features.ContactMessage;
using IResult = RecImage.Infrastructure.Commons.IResult;

namespace RecImage.Api.Controllers;

[Route("api/[controller]/[action]")]
[Produces("application/json")]
public class ContactController : Controller
{
    private readonly IMediator _mediator;

    public ContactController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IResult> Post([FromBody] ContactMessageQuery message,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(message, cancellationToken);
    }
}