using ImageToPuzzle.Controllers;
using ImageConverter.Models;
using Moq;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using ImageToPuzzle.Infrastructure.Logging;
using System;
using ImageToPuzzle.Services;
using ImageToPuzzle.Models;

namespace ImageToPuzzle.Test
{
	public class ImageConvertFromControll
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
			var loger = new Mock<IActionLoger>().Object;
			var imagesService = new Mock<IGetImagesService>().Object;

			var iterations = 10;
			for (int index = 0; index < iterations; index++)
			{
				using var imageConverter = new ImageConverter.ImageConverter();
				var imageToPointConverter = new ImageToPointService(imageConverter, imagesService);
				var controller = new GenerateController(imageToPointConverter, loger);
				var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files\\test_image.jpg");
				using var stream = new MemoryStream(File.ReadAllBytes(filePath));
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
			var loger = new Mock<IActionLoger>().Object;
			var imagesService = new Mock<IGetImagesService>().Object;
			using var imageConverter = new ImageConverter.ImageConverter();
			var imageToPointConverter = new ImageToPointService(imageConverter, imagesService);
			var controller = new GenerateController(imageToPointConverter, loger);
			await Assert.ThrowsAsync<NullReferenceException>(async () => await controller.ConvertToPoints(null, convertOptions));
		}

		[Fact]
		public void ConvertToPoint_ByFileName()
		{
			var convertOptions = new ConvertFromNameOptions()
			{
				Colored = true,
				ColorStep = ColorStep.VeryBig,
				Size = 300,
				ImageId = 1
			};

			var loger = new Mock<IActionLoger>().Object;
			var imagesService = new Mock<IGetImagesService>().Object;
			using var imageConverter = new ImageConverter.ImageConverter();
			var imageToPointConverter = new ImageToPointService(imageConverter, imagesService);
			var controller = new GenerateController(imageToPointConverter, loger);
			var result = controller.ConvertToPointsById(convertOptions);

			Assert.NotNull(result);
		}
	}
}

