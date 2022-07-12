using RecImage.Api.Constants;
using RecImage.Infrastructure.Logger;
using Serilog;

namespace RecImage.Api;

public class Program
{
    private static IConfiguration Configuration { get; } = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", false, true)
        .AddJsonFile(
            $"appsettings.{Environment.GetEnvironmentVariable(EnvironmentConstants.AspnetcoreEnvironment) ?? EnvironmentConstants.Production}.json",
            true)
        .AddUserSecrets<Program>(false, true)
        .Build();

    public static int Main(string[] args)
    {
        Log.Logger = Bootstrapper.GetSerilogLogger(Configuration);

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
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}