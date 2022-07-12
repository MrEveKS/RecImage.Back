using MediatR;
using RecImage.ColoringService.Enums;
using RecImage.Infrastructure.Commons;

namespace RecImage.Business.Features.ConvertToPointsById;

public sealed class ConvertToPointsByIdQuery
    : IRequest<IResult<ConvertToPointsByIdQueryResult>>
{
    public int ImageId { get; set; }
    public bool Colored { get; set; }
    public int Size { get; set; }
    public ColorStep ColorStep { get; set; }
}