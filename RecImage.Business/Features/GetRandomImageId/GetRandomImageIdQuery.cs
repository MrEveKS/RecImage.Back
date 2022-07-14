using MediatR;
using RecImage.Infrastructure.Commons;

namespace RecImage.Business.Features.GetRandomImageId;

public sealed record GetRandomImageIdQuery : IRequest<IResult<GetRandomImageIdQueryResult>>;