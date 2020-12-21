using ImageToPuzzle.Controllers;
using ImageConverter.Models;
using Moq;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace ImageToPuzzle.Test
{
	public class ImageConvertFromControll
	{
		[Fact]
		public async Task ConvertToChar_MemoryTest()
		{
			var convertOptions = new ConvertOptions()
			{
				Colored = true,
				ColorStep = ColorStep.VeryBig,
				Size = 300
			};

			var iterations = 1;
			for (int index = 0; index < iterations; index++)
			{
				using var imageConverter = new ImageConverter.ImageConverter();
				var controller = new GenerateController(imageConverter);
				var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files\\test_image.jpg");
				using var stream = new MemoryStream(File.ReadAllBytes(filePath));
				var formFile = new FormFile(stream, 0, stream.Length, "name", "test_image.jpg");

				var result = await controller.ConvertToChar(formFile, convertOptions);

				Assert.NotNull(result);
			}
		}
	}
}
