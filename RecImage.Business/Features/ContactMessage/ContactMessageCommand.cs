using MediatR;

namespace RecImage.Business.Features.ContactMessage;

public sealed record ContactMessageCommand(string UserName, string UserEmail, string UserMessage) : INotification;