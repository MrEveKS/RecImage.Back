using System.Collections.Generic;

namespace ImageService.Models;

public sealed class ColorPoints
{
	public List<List<int>> Cells { get; set; }

	public Dictionary<int, string> CellsColor { get; set; }
}