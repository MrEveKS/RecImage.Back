using RecImage.ColoringService.Enums;

namespace RecImage.ColoringService.Models;

public sealed class ConvertOptions
{
    public bool Colored { get; set; }
    public int Size { get; set; }
    public ColorStep ColorStep { get; set; }
}