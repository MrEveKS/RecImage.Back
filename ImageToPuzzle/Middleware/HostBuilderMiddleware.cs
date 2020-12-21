using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;

namespace ImageToPuzzle.Middleware
{
	public static class HostBuilderMiddleware
	{
		private static IConfiguration Configuration { get; } = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
			.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
			.Build();

		public static IHostBuilder UseWebPartstatCoreBuilder<TStartup>(this IHostBuilder builder)
			where TStartup : class
		{
			if (builder == null)
			{
				builder = new HostBuilder();
			}

			return AddDefaultConfiguration<TStartup>(builder);
		}

        private static IHostBuilder AddDefaultConfiguration<TStartup>(this IHostBuilder hostBuilder)
            where TStartup : class
        {
            hostBuilder.ConfigureAppConfiguration((hostContext, config) =>
            {
                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(Configuration)
                    .CreateLogger();

                var env = hostContext.HostingEnvironment;

                config.Sources.Clear();

                config.SetBasePath(env.ContentRootPath);
                config.AddJsonFile("appsettings.json", false, true);
                config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);

                config.AddEnvironmentVariables();

                if (env.IsDevelopment())
                {
                    config.AddUserSecrets<TStartup>();
                    config.AddEnvironmentVariables("Partstat");
                }
            });

            hostBuilder.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseKestrel()
                    .UseIISIntegration()
                    .UseStartup<TStartup>()
                    .CaptureStartupErrors(true)
                    .UseSetting("detailedErrors", "true");
            });

            return hostBuilder;
        }
    }
}
