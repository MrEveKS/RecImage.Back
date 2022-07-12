using MediatR;

namespace RecImage.Business.Features.ContactMessage;

public sealed class ContactMessageCommand : INotification
{
    public string? UserName { get; set; }
    public string? UserEmail { get; set; }
    public string? UserMessage { get; set; }
}