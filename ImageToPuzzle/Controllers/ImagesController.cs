using ImageToPuzzle.Infrastructure.Logging;
using ImageToPuzzle.Models;
using ImageToPuzzle.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ImageToPuzzle.Controllers
{
	[Route("api/[controller]/[action]")]
	public class ImagesController : Controller
	{
		private readonly IGetImagesService _imagesService;
		private readonly IActionLoger _loger;

		public ImagesController(IGetImagesService imagesService, IActionLoger loger)
		{
			_imagesService = imagesService;
			_loger = loger;
		}

		[HttpPost]
		public List<ImageListItem> GetAll()
		{
			try
			{
				Stopwatch stopwatch = Stopwatch.StartNew();
				var result = _imagesService.GetList();
				_loger.Information("GetAll time", stopwatch.Elapsed.TotalSeconds);
				return result;
			}
			catch (Exception ex)
			{
				_loger.Error(ex, "Images GetAll");
				throw;
			}
		}
	}
}
