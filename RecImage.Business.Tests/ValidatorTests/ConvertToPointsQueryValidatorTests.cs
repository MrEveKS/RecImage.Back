using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RecImage.Business.Features.ConvertToPoints;
using RecImage.Business.Tests.BaseTests;
using RecImage.ColoringService.Enums;
using RecImage.Test.Commons.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace RecImage.Business.Tests.ValidatorTests;

[Collection("Collection ConvertToPointsQueryValidatorTests")]
public sealed class ConvertToPointsQueryValidatorTests : BaseBusinessTests
{
    public ConvertToPointsQueryValidatorTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task Valid_Image_Must_Success_Query()
    {
        var source = new CancellationTokenSource();
        source.CancelAfter(StopAfter);

        try
        {
            var mediator = ServiceProvider.GetRequiredService<IMediator>();

            await using var stream = ImageGenerate.GenerateGradientImage();
            var formFile = new FormFile(stream, 0, stream.Length, "name", "test_image.jpg");

            var command = new ConvertToPointsQuery(formFile, true, 50, ColorStep.Middle);

            var result = await mediator.Send(command, source.Token);

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().BeNull();

            Output.WriteLine(JsonConvert.SerializeObject(result));
        }
        finally
        {
            source.Cancel();
            await ServiceProvider.DisposeAsync();
        }
    }

    [Fact]
    public async Task NotValid_Image_Must_NotSuccess_Query()
    {
        var source = new CancellationTokenSource();
        source.CancelAfter(StopAfter);

        try
        {
            var mediator = ServiceProvider.GetRequiredService<IMediator>();

            var command = new ConvertToPointsQuery(null, true, 50, ColorStep.Middle);

            var result = await mediator.Send(command, source.Token);

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().NotBeEmpty();

            Output.WriteLine(JsonConvert.SerializeObject(result));
        }
        finally
        {
            source.Cancel();
            await ServiceProvider.DisposeAsync();
        }
    }

    [Fact]
    public async Task Valid_Size_Must_Success_Query()
    {
        var source = new CancellationTokenSource();
        source.CancelAfter(StopAfter);

        try
        {
            const int size = 50;
            var mediator = ServiceProvider.GetRequiredService<IMediator>();

            await using var stream = ImageGenerate.GenerateGradientImage();
            var formFile = new FormFile(stream, 0, stream.Length, "name", "test_image.jpg");

            var command = new ConvertToPointsQuery(formFile, true, size, ColorStep.Middle);

            var result = await mediator.Send(command, source.Token);

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().BeNull();

            Output.WriteLine(JsonConvert.SerializeObject(result));
        }
        finally
        {
            source.Cancel();
            await ServiceProvider.DisposeAsync();
        }
    }

    [Fact]
    public async Task NotValid_Size_Must_NotSuccess_Query()
    {
        var source = new CancellationTokenSource();
        source.CancelAfter(StopAfter);

        try
        {
            const int size = 0;
            var mediator = ServiceProvider.GetRequiredService<IMediator>();

            await using var stream = ImageGenerate.GenerateGradientImage();
            var formFile = new FormFile(stream, 0, stream.Length, "name", "test_image.jpg");

            var command = new ConvertToPointsQuery(formFile, true, size, ColorStep.Middle);

            var result = await mediator.Send(command, source.Token);

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().NotBeEmpty();

            Output.WriteLine(JsonConvert.SerializeObject(result));
        }
        finally
        {
            source.Cancel();
            await ServiceProvider.DisposeAsync();
        }
    }
    
    [Fact]
    public async Task Valid_ColorStep_Must_Success_Query()
    {
        var source = new CancellationTokenSource();
        source.CancelAfter(StopAfter);

        try
        {
            const ColorStep colorStep = ColorStep.Middle;
            var mediator = ServiceProvider.GetRequiredService<IMediator>();

            await using var stream = ImageGenerate.GenerateGradientImage();
            var formFile = new FormFile(stream, 0, stream.Length, "name", "test_image.jpg");

            var command = new ConvertToPointsQuery(formFile, true, 50, colorStep);

            var result = await mediator.Send(command, source.Token);

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().BeNull();

            Output.WriteLine(JsonConvert.SerializeObject(result));
        }
        finally
        {
            source.Cancel();
            await ServiceProvider.DisposeAsync();
        }
    }

    [Fact]
    public async Task NotValid_ColorStep_Must_NotSuccess_Query()
    {
        var source = new CancellationTokenSource();
        source.CancelAfter(StopAfter);

        try
        {
            const ColorStep colorStep = (ColorStep)50;
            var mediator = ServiceProvider.GetRequiredService<IMediator>();

            await using var stream = ImageGenerate.GenerateGradientImage();
            var formFile = new FormFile(stream, 0, stream.Length, "name", "test_image.jpg");

            var command = new ConvertToPointsQuery(formFile, true, 50, colorStep);

            var result = await mediator.Send(command, source.Token);

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().NotBeEmpty();

            Output.WriteLine(JsonConvert.SerializeObject(result));
        }
        finally
        {
            source.Cancel();
            await ServiceProvider.DisposeAsync();
        }
    }
}