using ImageToPuzzle.Controllers;
using ImageConverter.Models;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Xunit;
using ImageToPuzzle.Infrastructure.Logging;
using System;
using System.Collections.Generic;
using ImageToPuzzle.Services;
using ImageToPuzzle.Models;
using ImageToPuzzle.Test.Helpers;

namespace ImageToPuzzle.Test
{
	public class ImageConvertFromController
	{
		[Fact]
		public async Task ConvertToPoint_MemoryTest()
		{
			var convertOptions = new ConvertOptions()
			{
				Colored = true,
				ColorStep = ColorStep.VeryBig,
				Size = 300
			};

			var logger = new Mock<IActionLogger>().Object;
			var fileService = new Mock<IFileService>().Object;
			var imagesService = new Mock<IGetImagesService>().Object;

			const int iterations = 10;

			for (var index = 0; index < iterations; index++)
			{
				using var imageConverter = new ImageConverter.ImageConverter();

				var imageToPointConverter = new ImageToPointService(imageConverter, imagesService, fileService);
				var controller = new GenerateController(imageToPointConverter, logger);
				await using var stream = ImageGenerate.GenerateGradientImage();
				var formFile = new FormFile(stream, 0, stream.Length, "name", "test_image.jpg");

				var result = await controller.ConvertToPoints(formFile, convertOptions);

				Assert.NotNull(result);
			}
		}

		[Fact]
		public async Task ConvertToPoint_NullExceptionTest()
		{
			var convertOptions = new ConvertOptions()
			{
				Colored = true,
				ColorStep = ColorStep.VeryBig,
				Size = 300
			};

			var logger = new Mock<IActionLogger>().Object;
			var fileService = new Mock<IFileService>().Object;
			var imagesService = new Mock<IGetImagesService>().Object;
			using var imageConverter = new ImageConverter.ImageConverter();
			var imageToPointConverter = new ImageToPointService(imageConverter, imagesService, fileService);
			var controller = new GenerateController(imageToPointConverter, logger);
			await Assert.ThrowsAsync<NullReferenceException>(async () => await controller.ConvertToPoints(null, convertOptions));
		}

		[Fact]
		public async Task ConvertToPoint_ByFileName()
		{
			var convertOptions = new ConvertFromNameOptions()
			{
				Colored = true,
				ColorStep = ColorStep.VeryBig,
				Size = 300,
				ImageId = 1
			};

			var logger = new Mock<IActionLogger>().Object;
			var imagesService = new Mock<IGetImagesService>();

			imagesService.Setup(x => x.GetList())
				.Returns(new List<ImageListItem>() { new ImageListItem() { Id = 1, Name = "test_image.jpg" } });

			var fileService = new Mock<IFileService>();

			fileService.Setup(f => f.OpenRead(It.IsAny<string>()))
				.Returns(ImageGenerate.GenerateGradientImage());

			using var imageConverter = new ImageConverter.ImageConverter();
			var imageToPointConverter = new ImageToPointService(imageConverter, imagesService.Object, fileService.Object);
			var controller = new GenerateController(imageToPointConverter, logger);
			var jsonResult = await controller.ConvertToPointsById(convertOptions);
			var recColor = jsonResult.Value as RecColor;

			Assert.NotNull(jsonResult);
			Assert.NotNull(recColor);
			Assert.True(recColor.Cells.Count > 0);
			Assert.True(recColor.CellsColor.Count > 0);
		}
	}
}