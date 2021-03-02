using ImageToPuzzle.Common.Constants;
using ImageToPuzzle.Models;
using System;
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
				Path.Combine(Directory.GetCurrentDirectory(), FolderConstant.ImageMinPath));
			var files = directoryInfo.GetFiles();
			return files
				.OrderBy(f => f.Name)
				.Select((f, i) => new ImageListItem()
				{
					Id = i + 1,
					Name = f.Name
				}).ToList();
		}

		public int GetRandomId()
		{
			var random = new Random();
			var directoryInfo = new DirectoryInfo(
				Path.Combine(Directory.GetCurrentDirectory(), FolderConstant.ImageMinPath));
			var filesCount = directoryInfo.GetFiles().Length;
			return random.Next(0, filesCount) + 1;
		}
	}
}
