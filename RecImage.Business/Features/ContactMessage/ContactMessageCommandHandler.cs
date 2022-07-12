using MediatR;
using RecImage.Infrastructure.Logger.Services;

namespace RecImage.Business.Features.ContactMessage;

internal sealed class ContactMessageCommandHandler
    : INotificationHandler<ContactMessageCommand>
{
    private readonly IActionLogger _logger;

    public ContactMessageCommandHandler(IActionLogger logger)
    {
        _logger = logger;
    }

    public Task Handle(ContactMessageCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.FatalObjectMessage(nameof(ContactMessageCommandHandler), request);
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.ErrorObject(ex, request);
            return Task.CompletedTask;
        }
    }
}