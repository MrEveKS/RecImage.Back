using RecImage.ColoringService.Models;

namespace RecImage.ColoringService.Services;

public interface IImageConverter
{
    Task<ColorPoints> ConvertToColorPoints(Stream imageStream, ConvertOptions options);
}