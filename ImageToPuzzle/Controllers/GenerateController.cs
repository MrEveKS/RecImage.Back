using ImageConverter.Models;
using ImageToPuzzle.Infrastructure.Logging;
using ImageToPuzzle.Models;
using ImageToPuzzle.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ImageToPuzzle.Controllers
{
	[Route("api/[controller]/[action]")]
	public class GenerateController : Controller
	{
		private readonly IImageToPointService _imageConverter;
		private readonly IActionLoger _loger;

		public GenerateController(IImageToPointService imageConverter, IActionLoger loger)
		{
			_imageConverter = imageConverter;
			_loger = loger;
		}

		/// <summary>
		/// Convert image to points
		/// </summary>
		/// <param name="image"> image from Form </param>
		/// <param name="options"> image setting for converter options </param>
		/// <returns></returns>
		[HttpPost]
		public async Task<RecColor> ConvertToPoints([FromForm] IFormFile image, [FromForm] ConvertOptions options)
		{
			try
			{
				_loger.InformationObject(options);
				Stopwatch stopwatch = Stopwatch.StartNew();
				var result = await _imageConverter.ConvertFromFile(image, options);
				_loger.Information("ConvertToPoints time", stopwatch.Elapsed.TotalSeconds);
				return result;
			}
			catch (Exception ex)
			{
				_loger.ErrorObject(ex, image);
				throw;
			}
		}

		[HttpPost]
		public async Task<RecColor> ConvertToPointsByFileName([FromForm] ConvertFromNameOptions options)
		{
			try
			{
				_loger.InformationObject(options);
				Stopwatch stopwatch = Stopwatch.StartNew();
				var result = await _imageConverter.ConvertFromFileName(options);
				_loger.Information("ConvertToPointsByFileName time", stopwatch.Elapsed.TotalSeconds);
				return result;
			}
			catch (Exception ex)
			{
				_loger.ErrorObject(ex, options);
				throw;
			}
		}
	}
}
