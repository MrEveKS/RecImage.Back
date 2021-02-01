using ImageToPuzzle.Controllers;
using ImageToPuzzle.Infrastructure.Logging;
using ImageToPuzzle.Services;
using Moq;
using Xunit;

namespace ImageToPuzzle.Test
{
	public class ImagesControllerTest
	{
		[Fact (Skip = "Local Test")]
		public void GetAll_Test()
		{
			var logger = new Mock<IActionLogger>().Object;
			var imagesService = new GetImagesService();
			var imagesConstroller = new ImagesController(imagesService, logger);

			var files = imagesConstroller.GetAll();

			Assert.NotNull(files);
			Assert.True(files.Value.ToString().Length > 0);
		}
	}
}
