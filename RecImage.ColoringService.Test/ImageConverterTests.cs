using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using RecImage.ColoringService.Enums;
using RecImage.ColoringService.Models;
using RecImage.ColoringService.Services;
using RecImage.ColoringService.Test.BaseTests;
using RecImage.ColoringService.Test.Helpers;
using RecImage.Infrastructure.Logger.Services;
using Xunit;
using Xunit.Abstractions;

namespace RecImage.ColoringService.Test;

[Collection("Collection ImageConverterTests")]
public class ImageConverterTests : BaseImageTests
{
    public ImageConverterTests(ITestOutputHelper output) : base(output)
    {
    }
    
    [Fact]
    public async Task ConvertToColorPoints_Should_Convert_And_ReturnResult()
    {
        var convertOptions = new ConvertOptions
        {
            Colored = true,
            ColorStep = ColorStep.VeryBig,
            Size = 300
        };

        var service = ServiceProvider.GetRequiredService<IImageConverter>();
            
        await using var stream = ImageGenerate.GenerateGradientImage();

        var result = await service.ConvertToColorPoints(stream, convertOptions);

        result.Should().NotBeNull();
        result.Cells.Should().NotBeEmpty();
        result.CellsColor.Should().NotBeEmpty();
            
        Output.WriteLine(JsonConvert.SerializeObject(result));
    }


}