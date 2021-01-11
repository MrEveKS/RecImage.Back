using ImageToPuzzle.Controllers;
using ImageToPuzzle.Infrastructure.Logging;
using ImageToPuzzle.Services;
using Moq;
using Xunit;

namespace ImageToPuzzle.Test
{
	public class ImagesControllerTest
	{
		[Fact]
		public void GetAll_Test()
		{
			var loger = new Mock<IActionLoger>().Object;
			var imagesService = new GetImagesService();
			var imagesConstroller = new ImagesController(imagesService, loger);

			var files = imagesConstroller.GetAll();

			Assert.NotNull(files);
			Assert.True(files.Count > 0);
		}
	}
}
