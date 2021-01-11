using ImageToPuzzle.Common.Constants;
using ImageToPuzzle.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ImageToPuzzle.Services
{
	public class GetImagesService : IGetImagesService
	{
		public List<ImageListItem> GetList()
		{
			var directoryInfo = new DirectoryInfo(
				Path.Combine(Directory.GetCurrentDirectory(), FolderConstant.ImagePath));
			var files = directoryInfo.GetFiles();
			return files
				.OrderBy(f => f)
				.Select((f, i) => new ImageListItem()
				{
					Id = i + 1,
					Name = f.Name
				}).ToList();
		}
	}
}
