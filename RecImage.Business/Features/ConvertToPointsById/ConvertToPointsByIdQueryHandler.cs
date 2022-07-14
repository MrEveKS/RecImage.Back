using System.Diagnostics;
using MediatR;
using RecImage.Business.Constants;
using RecImage.Business.Features.GetImagesList;
using RecImage.Business.Services;
using RecImage.ColoringService.Models;
using RecImage.ColoringService.Services;
using RecImage.Infrastructure.Commons;
using RecImage.Infrastructure.Logger.Services;

namespace RecImage.Business.Features.ConvertToPointsById;

internal sealed class ConvertToPointsByIdQueryHandler
    : IRequestHandler<ConvertToPointsByIdQuery, IResult<ConvertToPointsByIdQueryResult>>
{
    private readonly IFileService _fileService;
    private readonly IImageConverter _imageConverter;
    private readonly IActionLogger _logger;
    private readonly IMediator _mediator;

    public ConvertToPointsByIdQueryHandler(IFileService fileService,
        IImageConverter imageConverter,
        IActionLogger logger,
        IMediator mediator)
    {
        _fileService = fileService;
        _imageConverter = imageConverter;
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<IResult<ConvertToPointsByIdQueryResult>> Handle(
        ConvertToPointsByIdQuery request, CancellationToken cancellationToken)
    {
        var (imageId, colored, size, colorStep) = request;

        try
        {
            _logger.InformationObject(request);
            var stopwatch = Stopwatch.StartNew();

            var imagesResult = await _mediator.Send(new GetImagesListQuery(), cancellationToken);
            if (!imagesResult.Success || imagesResult.Data is null)
            {
                return Result<ConvertToPointsByIdQueryResult>
                    .Failed("Convert to point by id images not found");
            }

            var fileName = imagesResult.Data
                .FirstOrDefault(x => x.Id == imageId)?.OriginalName;

            if (string.IsNullOrEmpty(fileName))
            {
                return Result<ConvertToPointsByIdQueryResult>
                    .Failed($"Convert to point by id image not found by id {imageId}");
            }

            await using var stream = _fileService.OpenRead(Path.Combine(Directory.GetCurrentDirectory(),
                FolderConstant.ImagePath,
                fileName));

            var options = new ConvertOptions
                { Colored = colored, Size = size, ColorStep = colorStep };

            var result = await _imageConverter.ConvertToColorPoints(stream, options);
            _logger.Information("ConvertToPointsByFileName time", stopwatch.Elapsed.TotalMilliseconds);

            return Result<ConvertToPointsByIdQueryResult>
                .Ok(new ConvertToPointsByIdQueryResult(result.Cells, result.CellsColor));
        }
        catch (Exception e)
        {
            _logger.ErrorObject(e, request);
            return Result<ConvertToPointsByIdQueryResult>
                .Failed("Convert to point by id exception");
        }
    }
}