using System.Diagnostics;
using MediatR;
using RecImage.Business.Constants;
using RecImage.Business.Services;
using RecImage.Infrastructure.Commons;
using RecImage.Infrastructure.Logger.Services;

namespace RecImage.Business.Features.GetRandomImageId;

internal sealed class GetRandomImageIdQueryHandler
    : IRequestHandler<GetRandomImageIdQuery, IResult<GetRandomImageIdQueryResult>>
{
    private readonly IDirectoryService _directoryService;
    private readonly IActionLogger _logger;

    public GetRandomImageIdQueryHandler(IDirectoryService directoryService,
        IActionLogger logger)
    {
        _directoryService = directoryService;
        _logger = logger;
    }

    public Task<IResult<GetRandomImageIdQueryResult>> Handle(GetRandomImageIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            var id = GetRandomId();
            _logger.Information("GetRandomId time", stopwatch.Elapsed.TotalSeconds);

            return Task.FromResult(Result<GetRandomImageIdQueryResult>
                .Ok(new GetRandomImageIdQueryResult(id)));
        }
        catch (Exception e)
        {
            _logger.Error(e, nameof(GetRandomId));
            return Task.FromResult(Result<GetRandomImageIdQueryResult>
                .Failed("Get random id exception"));
        }
    }

    private int GetRandomId()
    {
        var random = new Random();
        var files = _directoryService
            .GetFiles(Path.Combine(Directory.GetCurrentDirectory(), FolderConstant.ImageMinWebpPath));

        var filesCount = files.Length;
        return random.Next(0, filesCount) + 1;
    }
}