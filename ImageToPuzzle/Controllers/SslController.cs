using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace ImageToPuzzle.Controllers;

public class SslController : Controller
{
	private const string KeyFile = "AD59A9C36DF4B7263B0C8C331B08AABF.txt";

	[HttpGet]
	[Route(".well-known/pki-validation/AD59A9C36DF4B7263B0C8C331B08AABF.txt")]
	public VirtualFileResult GetFile()
	{
		var filepath = Path.Combine("~/Files", KeyFile);

		return File(filepath, "text/plain", KeyFile);
	}
}