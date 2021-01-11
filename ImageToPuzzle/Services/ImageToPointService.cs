using ImageConverter;
using ImageConverter.Models;
using ImageToPuzzle.Common.Constants;
using ImageToPuzzle.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace ImageToPuzzle.Services
{
	public class ImageToPointService : IImageToPointService
	{
		private readonly IImageConverter _imageConverter;

		public ImageToPointService(IImageConverter imageConverter)
		{
			_imageConverter = imageConverter;
		}

		public async Task<RecColor> ConvertFromFile(IFormFile image, ConvertOptions options)
		{
			using var memoryStream = new MemoryStream();
			await image.CopyToAsync(memoryStream)
				.ConfigureAwait(AsyncConstant.ContinueOnCapturedContext);
			var result = await _imageConverter.ConvertToChars(memoryStream, options)
				.ConfigureAwait(AsyncConstant.ContinueOnCapturedContext);
			return result;
		}

		public async Task<RecColor> ConvertFromFileName(ConvertFromNameOptions options)
		{
			using var stream = File.OpenRead(Path.Combine(Directory.GetCurrentDirectory(),
				FolderConstant.ImagePath, options.FileName));
			var result = await _imageConverter.ConvertToChars(stream, options)
				.ConfigureAwait(AsyncConstant.ContinueOnCapturedContext);
			return result;
		}
	}
}
