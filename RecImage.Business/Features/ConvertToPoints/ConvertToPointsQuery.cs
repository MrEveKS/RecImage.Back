using MediatR;
using Microsoft.AspNetCore.Http;
using RecImage.ColoringService.Enums;
using RecImage.Infrastructure.Commons;

namespace RecImage.Business.Features.ConvertToPoints;

public sealed class ConvertToPointsQuery
    : IRequest<IResult<ConvertToPointsQueryResult>>
{
    public IFormFile Image { get; set; }
    public bool Colored { get; set; }
    public int Size { get; set; }
    public ColorStep ColorStep { get; set; }
}