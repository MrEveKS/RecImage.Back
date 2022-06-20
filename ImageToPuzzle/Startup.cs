using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using ImageService;
using ImageToPuzzle.Common.Constants;
using ImageToPuzzle.Errors;
using ImageToPuzzle.Infrastructure.Logging;
using ImageToPuzzle.Infrastructure.Providers;
using ImageToPuzzle.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;

namespace ImageToPuzzle;

public class Startup
{
	public Startup(IConfiguration configuration)
	{
		Configuration = configuration;
	}

	private IConfiguration Configuration { get; }

	public void ConfigureServices(IServiceCollection services)
	{
		var isDevelop = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

		services.AddCors(options =>
		{
			options.AddPolicy("TestPolicy",
				builder => builder
					.WithOrigins("http://localhost:4200", "https://localhost:4200", "http://192.168.0.103:8080")
					.AllowAnyOrigin()
					.AllowAnyMethod()
					.AllowAnyHeader());

			options.AddPolicy("ProductionPolicy",
				builder => builder
					.WithOrigins("https://recimage.ru",
						"http://recimage.ru",
						"https://www.recimage.ru",
						"http://www.recimage.ru")
					.AllowAnyOrigin()
					.AllowAnyMethod()
					.AllowAnyHeader());
		});

		if (isDevelop)
		{
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1",
					new OpenApiInfo
					{
						Title = "Test API",
						Version = "v1",
						Contact = new OpenApiContact
						{
							Name = "Git Hub",
							Email = string.Empty
						}
					});

				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

				if (File.Exists(xmlPath))
				{
					c.IncludeXmlComments(xmlPath);
				}
			});
		}

		services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
		services.Configure<BrotliCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);

		services.AddResponseCompression(options =>
		{
			options.EnableForHttps = true;
			options.Providers.Add<GzipCompressionProvider>();
			options.Providers.Add<BrotliCompressionProvider>();
			options.Providers.Add<DeflateCompressionProvider>();

			options.MimeTypes =
				ResponseCompressionDefaults.MimeTypes.Concat(new[]
				{
					"application/javascript",
					"application/json",
					"application/xml",
					"text/css",
					"text/html",
					"text/json",
					"text/plain",
					"text/xml",
					"image/png"
				});
		});

		services.AddSingleton(Configuration)
			.AddScoped<IFileService, FileService>()
			.AddScoped<IDirectoryService, DirectoryService>()
			.AddImageServices()
			.AddScoped<IActionLogger, ActionLogger>()
			.AddScoped<IImageToPointService, ImageToPointService>()
			.AddScoped<IGetImagesService, GetImagesService>();

		services.AddMvcCore(options =>
			{
				options.RespectBrowserAcceptHeader = true;
			})
			.AddApiExplorer();
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		} else
		{
			app.UseExceptionHandler("/error");
			app.UseHsts();
		}

		app.UseResponseCompression();

		if (env.IsDevelopment())
		{
			app.UseSwagger();

			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API V1");
				c.RoutePrefix = string.Empty;

				c.OAuthClientId("swagger-ui");
				c.OAuthClientSecret("swagger-ui-secret");
				c.OAuthRealm("swagger-ui-realm");
				c.OAuthAppName("Swagger UI");
			});
		}

		if (!env.IsDevelopment())
		{
			app.UseHttpsRedirection();
		}

		app.UseStaticFiles();
		app.UseSerilogRequestLogging();

		app.UseRouting();
		app.UseCors(env.IsDevelopment() ? "TestPolicy" : "ProductionPolicy");

		app.UseEndpoints(endpoints =>
		{
			endpoints.MapControllers()
				.RequireCors(env.IsDevelopment() ? "TestPolicy" : "ProductionPolicy");
		});

		app.Map("/error",
			ap => ap.Run(async context =>
			{
				context.Response.ContentType = "application/json";
				var response = JsonConvert.SerializeObject(new ErrorResponse("Exception", context.Response.StatusCode));

				await context.Response
					.WriteAsync(response)
					.ConfigureAwait(AsyncConstant.ContinueOnCapturedContext);
			}));

		app.UseStatusCodePages(async context =>
		{
			context.HttpContext.Response.ContentType = "application/json";
			var response = string.Empty;

			if (context.HttpContext.Response.StatusCode == 404)
			{
				response = JsonConvert.SerializeObject(new ErrorResponse("Page not found", 404));
			}

			await context.HttpContext.Response
				.WriteAsync(response)
				.ConfigureAwait(AsyncConstant.ContinueOnCapturedContext);
		});
	}
}