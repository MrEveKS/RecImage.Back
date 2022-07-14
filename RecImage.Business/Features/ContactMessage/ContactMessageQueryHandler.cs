using MediatR;
using RecImage.Infrastructure.Commons;
using RecImage.Infrastructure.Logger.Services;

namespace RecImage.Business.Features.ContactMessage;

internal sealed class ContactMessageQueryHandler
    : IRequestHandler<ContactMessageQuery, IResult>
{
    private readonly IActionLogger _logger;

    public ContactMessageQueryHandler(IActionLogger logger)
    {
        _logger = logger;
    }

    public Task<IResult> Handle(ContactMessageQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.FatalObjectMessage(nameof(ContactMessageQueryHandler), request);
            return Task.FromResult(Result.Ok());
        }
        catch (Exception ex)
        {
            _logger.ErrorObject(ex, request);
            return Task.FromResult(Result.Failed("Send message exception"));
        }
    }
}