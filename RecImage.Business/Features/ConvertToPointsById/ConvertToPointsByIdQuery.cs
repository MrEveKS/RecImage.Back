using MediatR;
using RecImage.ColoringService.Enums;
using RecImage.Infrastructure.Commons;

namespace RecImage.Business.Features.ConvertToPointsById;

public sealed record ConvertToPointsByIdQuery(int ImageId, bool Colored, int Size, ColorStep ColorStep)
    : IRequest<IResult<ConvertToPointsByIdQueryResult>>;