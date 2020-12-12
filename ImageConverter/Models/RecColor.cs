using System.Collections.Generic;

namespace ImageConverter.Models
{
	public class RecColor
	{
		public List<List<int>> Cells { get; set; }
		public Dictionary<int, string> CellsColor { get; set; }
	}
}
