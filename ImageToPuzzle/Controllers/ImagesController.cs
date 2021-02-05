using ImageToPuzzle.Infrastructure.Logging;
using ImageToPuzzle.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace ImageToPuzzle.Controllers
{
	[Route("api/[controller]/[action]")]
	[Produces("application/json")]
	public class ImagesController : Controller
	{
		private readonly IGetImagesService _imagesService;
		private readonly IActionLogger _logger;

		public ImagesController(IGetImagesService imagesService, IActionLogger logger)
		{
			_imagesService = imagesService;
			_logger = logger;
		}

		[HttpPost]
		public JsonResult GetAll()
		{
			try
			{
				Stopwatch stopwatch = Stopwatch.StartNew();
				var result = _imagesService.GetList();
				_logger.Information("GetAll time", stopwatch.Elapsed.TotalSeconds);
				return new JsonResult(result);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, nameof(ImagesController.GetAll));
				throw;
			}
		}

		[HttpPost]
		public JsonResult GetRandomId()
		{
			try
			{
				Stopwatch stopwatch = Stopwatch.StartNew();
				var result = _imagesService.GetRandomId();
				_logger.Information("GetRandomId time", stopwatch.Elapsed.TotalSeconds);
				return new JsonResult(result);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, nameof(ImagesController.GetRandomId));
				throw;
			}
		}
	}
}
