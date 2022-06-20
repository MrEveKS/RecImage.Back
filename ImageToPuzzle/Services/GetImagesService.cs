using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ImageToPuzzle.Common.Constants;
using ImageToPuzzle.Models;

namespace ImageToPuzzle.Services;

internal sealed class GetImagesService : IGetImagesService
{
	private readonly IDirectoryService _directoryService;

	public GetImagesService(IDirectoryService directoryService)
	{
		_directoryService = directoryService;
	}

	public List<ImageListItem> GetList()
	{
		var files = _directoryService.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), FolderConstant.ImageMinPath));

		return files
			.OrderBy(f => f.Name)
			.Select((f, i) => new ImageListItem
			{
				Id = i + 1,
				Name = f.Name
			})
			.ToList();
	}

	public int GetRandomId()
	{
		var random = new Random();
		var files = _directoryService.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), FolderConstant.ImageMinPath));
		var filesCount = files.Length;

		return random.Next(0, filesCount) + 1;
	}
}