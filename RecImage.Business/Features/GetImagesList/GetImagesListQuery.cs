using MediatR;
using RecImage.Infrastructure.Commons;

namespace RecImage.Business.Features.GetImagesList;

public sealed record GetImagesListQuery : IRequest<IResult<IReadOnlyCollection<GetImagesListQueryResult>>>;