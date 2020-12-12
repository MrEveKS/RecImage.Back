using System.Collections.Generic;

namespace ImageToPuzzle.Models
{
	public class RecColor
	{
		public List<List<string>> Cells { get; set; }
		public Dictionary<string, string> CellsColor { get; set; }
	}
}
