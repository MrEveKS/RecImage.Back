using MediatR;
using RecImage.Infrastructure.Commons;

namespace RecImage.Business.Features.GetRandomImageId;

public sealed class GetRandomImageIdQuery
    : IRequest<IResult<GetRandomImageIdQueryResult>>
{
}