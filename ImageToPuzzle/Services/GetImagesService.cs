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
		var files = _directoryService
			.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), FolderConstant.ImageMinWebpPath));

		var filesOriginal = _directoryService
			.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), FolderConstant.ImagePath));

		var collection = files.Join(filesOriginal,
			x => GetFileNameWithoutExtension(x.Name),
			x => GetFileNameWithoutExtension(x.Name),
			(x, y) => new ImageListItem
			{
				Name = x.Name,
				OriginalName = y.Name
			});

		return collection
			.OrderBy(f => f.Name)
			.Select((f, i) =>
			{
				f.Id = i + 1;
				return f;
			})
			.ToList();
	}

	public int GetRandomId()
	{
		var random = new Random();

		var files = _directoryService
			.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), FolderConstant.ImageMinWebpPath));

		var filesCount = files.Length;

		return random.Next(0, filesCount) + 1;
	}

	private string GetFileNameWithoutExtension(string fileName)
	{
		return Path.GetFileNameWithoutExtension(fileName);
	}
}