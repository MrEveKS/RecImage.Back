using MediatR;
using Microsoft.AspNetCore.Http;
using RecImage.ColoringService.Enums;
using RecImage.Infrastructure.Commons;

namespace RecImage.Business.Features.ConvertToPoints;

public sealed record ConvertToPointsQuery(IFormFile Image, bool Colored, int Size, ColorStep ColorStep)
    : IRequest<IResult<ConvertToPointsQueryResult>>;