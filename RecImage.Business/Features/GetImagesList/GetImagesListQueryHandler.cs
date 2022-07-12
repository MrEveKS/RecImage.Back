using System.Diagnostics;
using MediatR;
using RecImage.Business.Constants;
using RecImage.Business.Services;
using RecImage.Infrastructure.Commons;
using RecImage.Infrastructure.Logger.Services;

namespace RecImage.Business.Features.GetImagesList;

internal sealed class GetImagesListQueryHandler
    : IRequestHandler<GetImagesListQuery, IResult<IReadOnlyCollection<GetImagesListQueryResult>>>
{
    private readonly IDirectoryService _directoryService;
    private readonly IActionLogger _logger;

    public GetImagesListQueryHandler(IDirectoryService directoryService,
        IActionLogger logger)
    {
        _directoryService = directoryService;
        _logger = logger;
    }

    public Task<IResult<IReadOnlyCollection<GetImagesListQueryResult>>> Handle(
        GetImagesListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            var files = _directoryService
                .GetFiles(Path.Combine(Directory.GetCurrentDirectory(), FolderConstant.ImageMinWebpPath));
            var filesOriginal = _directoryService
                .GetFiles(Path.Combine(Directory.GetCurrentDirectory(), FolderConstant.ImagePath));

            var collection = files.Join(filesOriginal,
                x => GetFileNameWithoutExtension(x.Name),
                x => GetFileNameWithoutExtension(x.Name),
                (x, y) => new GetImagesListQueryResult
                {
                    Name = x.Name,
                    OriginalName = y.Name
                });

            var resultData = collection
                .OrderBy(f => f.Name)
                .Select((f, i) =>
                {
                    f.Id = i + 1;
                    return f;
                })
                .ToList();

            _logger.Information("GetAll images time", stopwatch.Elapsed.TotalSeconds);
            return Task.FromResult(Result<IReadOnlyCollection<GetImagesListQueryResult>>
                .Ok(resultData));
        }
        catch (Exception e)
        {
            _logger.Error(e, nameof(GetImagesListQueryHandler));
            return Task.FromResult(Result<IReadOnlyCollection<GetImagesListQueryResult>>
                .Failed("Get images exception"));
        }
    }

    private static string GetFileNameWithoutExtension(string fileName)
    {
        return Path.GetFileNameWithoutExtension(fileName);
    }
}