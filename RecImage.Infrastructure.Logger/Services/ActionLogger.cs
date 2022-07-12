using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;

namespace RecImage.Infrastructure.Logger.Services;

internal sealed class ActionLogger : IActionLogger
{
    private readonly ILogger _logger;

    public ActionLogger()
    {
        _logger = Log.Logger;
    }

    public void Information(string message, object value)
    {
        _logger.Information("{Message}: {Value}",
            message, value);
    }

    public void InformationObject<T>(T obj)
    {
        _logger.Information("{@Type} {Name}: {Value}",
            typeof(T), nameof(obj), JsonConvert.SerializeObject(obj));
    }

    public void Error(Exception exception, string message)
    {
        _logger.Error(exception, "message: {Msg}",
            message);
    }

    public void ErrorObject<T>(Exception exception, T obj)
    {
        switch (obj)
        {
            case null:
                _logger.Error(exception, "{@Type} {Name}: {Value}",
                    typeof(T), nameof(obj), "value is null");
                break;
            case IFormFile formFile:
                ErrorFormFile(exception, formFile);
                break;
            default:
                _logger.Error(exception, "{@Type} {Name}",
                    typeof(T), nameof(obj));
                break;
        }
    }

    public void FatalObjectMessage<T>(string message, T obj)
    {
        _logger.Fatal("{Message}: {@Type} {Name}: {Value}",
            message, typeof(T), nameof(obj), JsonConvert.SerializeObject(obj));
    }

    public void InformationObject<T>(string message, T obj)
    {
        _logger.Information("{Message}: {@Type} {Name}: {Value}",
            message, typeof(T), nameof(obj), JsonConvert.SerializeObject(obj));
    }

    private void ErrorFormFile(Exception exception, IFormFile formFile)
    {
        _logger.Error(exception,
            "{@Type} {Name}: ContentType: {ContentType}, FileName: {FileName}, Length: {Length}",
            typeof(IFormFile), nameof(formFile), formFile.ContentType, formFile.FileName, formFile.Length);
    }
}