using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecImage.Business.Features.ContactMessage;

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
    public async Task Post([FromBody] ContactMessageCommand message,
        CancellationToken cancellationToken = default)
    {
        await _mediator.Publish(message, cancellationToken);
    }
}