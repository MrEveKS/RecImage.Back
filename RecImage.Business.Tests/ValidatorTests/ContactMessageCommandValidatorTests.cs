using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RecImage.Business.Features.ContactMessage;
using RecImage.Business.Tests.BaseTests;
using Xunit;
using Xunit.Abstractions;

namespace RecImage.Business.Tests.ValidatorTests;

[Collection("Collection ContactMessageCommandValidatorTests")]
public sealed class ContactMessageCommandValidatorTests : BaseBusinessTests
{
    public ContactMessageCommandValidatorTests(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData("user@yandex.ru")]
    [InlineData("user@gmail.com")]
    [InlineData("user.name@g.mail.com")]
    public async Task Valid_UserEmail_Must_Success_Query(string userEmail)
    {
        var source = new CancellationTokenSource();
        source.CancelAfter(StopAfter);

        try
        {
            var mediator = ServiceProvider.GetRequiredService<IMediator>();

            var command = new ContactMessageQuery("name", userEmail, "message");

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

    [Theory]
    [InlineData("useryandexru")]
    [InlineData(null)]
    [InlineData("user.name.mailcom")]
    public async Task NotValid_UserEmail_Must_NotSuccess_Query(string userEmail)
    {
        var source = new CancellationTokenSource();
        source.CancelAfter(StopAfter);

        try
        {
            var mediator = ServiceProvider.GetRequiredService<IMediator>();

            var command = new ContactMessageQuery("name", userEmail, "message");

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

    [Theory]
    [InlineData("username")]
    [InlineData("  username")]
    public async Task Valid_UserName_Must_Success_Query(string userName)
    {
        var source = new CancellationTokenSource();
        source.CancelAfter(StopAfter);

        try
        {
            var mediator = ServiceProvider.GetRequiredService<IMediator>();

            var command = new ContactMessageQuery(userName, "user@yandex.ru", "message");

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

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public async Task NotValid_UserName_Must_NotSuccess_Query(string userName)
    {
        var source = new CancellationTokenSource();
        source.CancelAfter(StopAfter);

        try
        {
            var mediator = ServiceProvider.GetRequiredService<IMediator>();

            var command = new ContactMessageQuery(userName, "user@yandex.ru", "message");

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

    [Theory]
    [InlineData("message")]
    [InlineData("  message")]
    public async Task Valid_UserMessage_Must_Success_Query(string userMessage)
    {
        var source = new CancellationTokenSource();
        source.CancelAfter(StopAfter);

        try
        {
            var mediator = ServiceProvider.GetRequiredService<IMediator>();

            var command = new ContactMessageQuery("user", "user@yandex.ru", userMessage);

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

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public async Task NotValid_UserMessage_Must_NotSuccess_Query(string userMessage)
    {
        var source = new CancellationTokenSource();
        source.CancelAfter(StopAfter);

        try
        {
            var mediator = ServiceProvider.GetRequiredService<IMediator>();

            var command = new ContactMessageQuery("name", "user@yandex.ru", userMessage);

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