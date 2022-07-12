using System.Diagnostics;
using MediatR;
using RecImage.ColoringService.Models;
using RecImage.ColoringService.Services;
using RecImage.Infrastructure.Commons;
using RecImage.Infrastructure.Logger.Services;

namespace RecImage.Business.Features.ConvertToPoints;

internal sealed class ConvertToPointsQueryHandler
    : IRequestHandler<ConvertToPointsQuery, IResult<ConvertToPointsQueryResult>>
{
    private readonly IImageConverter _imageConverter;
    private readonly IActionLogger _logger;

    public ConvertToPointsQueryHandler(IImageConverter imageConverter,
        IActionLogger logger)
    {
        _imageConverter = imageConverter;
        _logger = logger;
    }

    public async Task<IResult<ConvertToPointsQueryResult>> Handle(
        ConvertToPointsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var options = new ConvertOptions
                { Colored = request.Colored, Size = request.Size, ColorStep = request.ColorStep };
            _logger.InformationObject(options);

            var stopwatch = Stopwatch.StartNew();
            await using var memoryStream = new MemoryStream();
            await request.Image.CopyToAsync(memoryStream, cancellationToken);
            var result = await _imageConverter.ConvertToColorPoints(memoryStream, options);
            _logger.Information("ConvertToPoints time", stopwatch.Elapsed.TotalMilliseconds);

            return Result<ConvertToPointsQueryResult>
                .Ok(new ConvertToPointsQueryResult { Cells = result.Cells, CellsColor = result.CellsColor });
        }
        catch (Exception e)
        {
            _logger.ErrorObject(e, request.Image);
            return Result<ConvertToPointsQueryResult>
                .Failed("Convert to points exception");
        }
    }
}