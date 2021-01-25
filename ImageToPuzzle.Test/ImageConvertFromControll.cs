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
using Newtonsoft.Json;

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
			var logger = new Mock<IActionLogger>().Object;
			var imagesService = new Mock<IGetImagesService>().Object;

			var iterations = 10;
			for (int index = 0; index < iterations; index++)
			{
				using var imageConverter = new ImageConverter.ImageConverter();
				var imageToPointConverter = new ImageToPointService(imageConverter, imagesService);
				var controller = new GenerateController(imageToPointConverter, logger);
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
			var logger = new Mock<IActionLogger>().Object;
			var imagesService = new Mock<IGetImagesService>().Object;
			using var imageConverter = new ImageConverter.ImageConverter();
			var imageToPointConverter = new ImageToPointService(imageConverter, imagesService);
			var controller = new GenerateController(imageToPointConverter, logger);
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

			var logger = new Mock<IActionLogger>().Object;
			var imagesService = new Mock<IGetImagesService>().Object;
			using var imageConverter = new ImageConverter.ImageConverter();
			var imageToPointConverter = new ImageToPointService(imageConverter, imagesService);
			var controller = new GenerateController(imageToPointConverter, logger);
			var result = controller.ConvertToPointsById(convertOptions);

			Assert.NotNull(result);
		}

		/// <summary>
		/// Test - getting string size
		/// </summary>
		/// <returns></returns>
		[Fact]
		public async Task ConvertToPoint_Size()
		{
			var convertOptions = new ConvertOptions()
			{
				Colored = true,
				ColorStep = ColorStep.VeryBig,
				Size = 300
			};
			var logger = new Mock<IActionLogger>().Object;
			var imagesService = new Mock<IGetImagesService>().Object;

			using var imageConverter = new ImageConverter.ImageConverter();
			var imageToPointConverter = new ImageToPointService(imageConverter, imagesService);
			var controller = new GenerateController(imageToPointConverter, logger);
			var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files\\test_image.jpg");
			using var stream = new MemoryStream(File.ReadAllBytes(filePath));
			var formFile = new FormFile(stream, 0, stream.Length, "name", "test_image.jpg");

			var result = await controller.ConvertToPoints(formFile, convertOptions);
			var json = JsonConvert.SerializeObject(result, Formatting.None);
			// 214600
			var jsonLength = JsonConvert.SerializeObject(result, Formatting.None).Length;

			// File.WriteAllText("test.json", json);

			Assert.NotNull(result);
		}
	}
}

