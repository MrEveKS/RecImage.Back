using MediatR;
using RecImage.Infrastructure.Commons;

namespace RecImage.Business.Features.GetImagesList;

public sealed class GetImagesListQuery
    : IRequest<IResult<IReadOnlyCollection<GetImagesListQueryResult>>>
{
}