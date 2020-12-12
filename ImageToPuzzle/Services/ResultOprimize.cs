using ImageConverter.Models;
using ImageToPuzzle.Models;
using System.Collections.Generic;

namespace ImageToPuzzle.Services
{
	public class ResultOprimize : IResultOprimize
	{
		public RecColor Convert(List<List<ColoredChar>> coloredChars)
		{
			var result = new RecColor()
			{
				Cells = new List<List<string>>(coloredChars.Count),
				CellsColor = new Dictionary<string, string>()
			};

			for (int indexRow = 0; indexRow < coloredChars.Count; indexRow++)
			{
				var row = coloredChars[indexRow];
				var rowResult = new List<string>(row.Count);
				for (int indexCell = 0; indexCell < row.Count; indexCell++)
				{
					var cell = row[indexCell];
					rowResult.Add(cell.Character);
					if (!result.CellsColor.ContainsKey(cell.Character))
					{
						result.CellsColor.Add(cell.Character, cell.WebColor);
					}
				}
				result.Cells.Add(rowResult);
			}

			return result;
		}
	}
}
