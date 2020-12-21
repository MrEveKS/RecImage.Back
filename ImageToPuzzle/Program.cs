using ImageToPuzzle.Middleware;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ImageToPuzzle
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		private static IHostBuilder CreateHostBuilder(string[] args)
		{
			return Host.CreateDefaultBuilder(args)
				.UseWebPartstatCoreBuilder<Startup>();
		}
	}
}
