using System.Collections.Generic;
using ImageToPuzzle.Controllers;
using ImageToPuzzle.Infrastructure.Logging;
using ImageToPuzzle.Models;
using ImageToPuzzle.Services;
using Moq;
using Xunit;

namespace ImageToPuzzle.Test;

public class ImagesControllerTest
{
	[Fact]
	public void GetAll_Test()
	{
		var logger = new Mock<IActionLogger>().Object;
		var directoryService = new Mock<IDirectoryService>().Object;
		var imagesService = new GetImagesService(directoryService);
		var imagesController = new ImagesController(imagesService, logger);

		var jsonResult = imagesController.GetAll();
		var files = jsonResult.Value as List<ImageListItem>;

		Assert.NotNull(jsonResult);
		Assert.NotNull(files);
	}
}