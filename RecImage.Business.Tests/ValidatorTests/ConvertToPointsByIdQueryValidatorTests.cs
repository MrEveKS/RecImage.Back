using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RecImage.Business.Features.ConvertToPointsById;
using RecImage.Business.Tests.BaseTests;
using RecImage.ColoringService.Enums;
using Xunit;
using Xunit.Abstractions;

namespace RecImage.Business.Tests.ValidatorTests;

[Collection("Collection ConvertToPointsQueryValidatorTests")]
public class ConvertToPointsByIdQueryValidatorTests : BaseBusinessTests
{
    public ConvertToPointsByIdQueryValidatorTests(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public async Task Valid_ImageId_Must_Success_Query()
    {
        var source = new CancellationTokenSource();
        source.CancelAfter(StopAfter);

        try
        {
            const int imageId = 1;
            var mediator = ServiceProvider.GetRequiredService<IMediator>();

            var command = new ConvertToPointsByIdQuery(imageId, true, 50, ColorStep.Middle);

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
    public async Task NotValid_ImageId_Must_NotSuccess_Query()
    {
        var source = new CancellationTokenSource();
        source.CancelAfter(StopAfter);

        try
        {
            const int imageId = -1;
            var mediator = ServiceProvider.GetRequiredService<IMediator>();

            var command = new ConvertToPointsByIdQuery(imageId, true, 50, ColorStep.Middle);

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

            var command = new ConvertToPointsByIdQuery(1, true, size, ColorStep.Middle);

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

            var command = new ConvertToPointsByIdQuery(1, true, size, ColorStep.Middle);

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

            var command = new ConvertToPointsByIdQuery(1, true, 50, colorStep);

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

            var command = new ConvertToPointsByIdQuery(1, true, 50, colorStep);

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