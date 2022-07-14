using MediatR;
using RecImage.Infrastructure.Commons;

namespace RecImage.Business.Features.ContactMessage;

public sealed record ContactMessageQuery(string UserName, string UserEmail, string UserMessage)
    : IRequest<IResult>;