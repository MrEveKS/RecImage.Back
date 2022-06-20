using ImageService.Models;

namespace ImageToPuzzle.Models;

public sealed class ConvertFromNameOptions : ConvertOptions
{
	public int ImageId { get; set; }
}