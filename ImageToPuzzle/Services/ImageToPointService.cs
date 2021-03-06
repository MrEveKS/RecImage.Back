using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageConverter;
using ImageConverter.Models;
using ImageToPuzzle.Common.Constants;
using ImageToPuzzle.Models;
using Microsoft.AspNetCore.Http;

namespace ImageToPuzzle.Services
{
	public class ImageToPointService : IImageToPointService
	{
		private readonly IFileService _fileService;

		private readonly IImageConverter _imageConverter;

		private readonly IGetImagesService _imagesService;

		public ImageToPointService(IImageConverter imageConverter, IGetImagesService imagesService, IFileService fileService)
		{
			_imageConverter = imageConverter;
			_imagesService = imagesService;
			_fileService = fileService;
		}

		public async Task<RecColor> ConvertFromFile(IFormFile image, ConvertOptions options)
		{
			await using var memoryStream = new MemoryStream();

			await image.CopyToAsync(memoryStream)
				.ConfigureAwait(AsyncConstant.ContinueOnCapturedContext);

			var result = await _imageConverter.ConvertToChars(memoryStream, options)
				.ConfigureAwait(AsyncConstant.ContinueOnCapturedContext);

			return result;
		}

		public async Task<RecColor> ConvertFromFileName(ConvertFromNameOptions options)
		{
			var images = _imagesService.GetList();
			var fileName = images.FirstOrDefault(x => x.Id == options.ImageId)?.Name;

			if (string.IsNullOrEmpty(fileName))
			{
				return null;
			}

			await using var stream = _fileService.OpenRead(Path.Combine(Directory.GetCurrentDirectory(),
				FolderConstant.ImagePath,
				fileName));

			var result = await _imageConverter.ConvertToChars(stream, options)
				.ConfigureAwait(AsyncConstant.ContinueOnCapturedContext);

			return result;
		}
	}
}