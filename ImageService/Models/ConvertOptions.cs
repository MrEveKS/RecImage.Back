using ImageService.Enums;

namespace ImageService.Models;

public class ConvertOptions
{
	public bool Colored { get; set; }
	public int Size { get; set; }
	public ColorStep ColorStep { get; set; }
}