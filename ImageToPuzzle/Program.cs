using System;
using System.IO;
using ImageToPuzzle.Common.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ImageToPuzzle;

public class Program
{
	private static IConfiguration Configuration { get; } = new ConfigurationBuilder()
		.SetBasePath(Directory.GetCurrentDirectory())
		.AddJsonFile("appsettings.json", false, true)
		.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
		.Build();

	public static int Main(string[] args)
	{
		Log.Logger = new LoggerConfiguration()
			.ReadFrom.Configuration(Configuration)
			.SetTelegramLogger(Configuration)
			.CreateLogger();

		try
		{
			Log.Information("Starting host");

			CreateHostBuilder(args)
				.Build()
				.Run();

			return 0;
		}
		catch (Exception ex)
		{
			Log.Fatal(ex, "Host terminated unexpectedly");

			return 1;
		}
		finally
		{
			Log.CloseAndFlush();
		}
	}

	private static IHostBuilder CreateHostBuilder(string[] args)
	{
		return Host.CreateDefaultBuilder(args)
			.UseSerilog()
			.ConfigureWebHostDefaults(webBuilder =>
			{
				webBuilder.UseStartup<Startup>();
			});
	}
}