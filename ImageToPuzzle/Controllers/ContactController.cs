using ImageToPuzzle.Infrastructure.Logging;
using ImageToPuzzle.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ImageToPuzzle.Controllers
{
	[Route("api/[controller]/[action]")]
	[Produces("application/json")]
	public class ContactController : Controller
	{
		private readonly IActionLogger _logger;

		public ContactController(IActionLogger logger)
		{
			_logger = logger;
		}

		[HttpPost]
		public void Post([FromBody] ContactMessage message)
		{
			try
			{
				_logger.FatalObjectMessage(nameof(ContactController.Post), message);
			}
			catch (Exception ex)
			{
				_logger.ErrorObject(ex, message);
				throw;
			}

		}
	}
}
