using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ImageConverter;
using ImageToPuzzle.Errors;
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

namespace ImageToPuzzle
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddScoped<IImageConverter, ImageConverter.ImageConverter>();
			services.AddScoped<IResultOprimize, ResultOprimize>();

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "Test API",
					Version = "v1",
					Contact = new OpenApiContact
					{
						Name = "Git Hub",
						Email = string.Empty,
					}
				});

				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				if (File.Exists(xmlPath))
				{
					c.IncludeXmlComments(xmlPath);
				}
			});

			services.AddCors(options =>
			{
				options.AddPolicy("TestPolicy", builder => builder
					.WithOrigins("http://localhost:4200", "https://localhost:4200")
					.AllowAnyHeader()
					.AllowAnyMethod());
				options.AddPolicy("ProductionPolicy", builder => builder
					.WithOrigins("https://recimage.ru", "http://recimage.ru",
						"https://www.recimage.ru", "www.http://recimage.ru")
					.AllowAnyHeader()
					.AllowAnyMethod());
			});

			services.AddResponseCompression(options =>
			{
				options.EnableForHttps = true;
				options.Providers.Add<GzipCompressionProvider>();
				options.Providers.Add<BrotliCompressionProvider>();
				options.Providers.Add<DeflateCompressionProvider>();
				options.MimeTypes =
					ResponseCompressionDefaults.MimeTypes.Concat(
						new[] {
							"application/javascript",
							"application/json",
							"application/xml",
							"text/css",
							"text/html",
							"text/json",
							"text/plain",
							"text/xml"
						});
			});

			services.AddMvcCore()
				.AddNewtonsoftJson()
				.AddApiExplorer();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseResponseCompression();
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

			if (!env.IsDevelopment())
			{
				app.UseHttpsRedirection();
			}
			app.UseStaticFiles();
			app.UseRouting();
			app.UseCors(env.IsDevelopment() ? "TestPolicy" : "ProductionPolicy");
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers().RequireCors(env.IsDevelopment() ? "TestPolicy" : "ProductionPolicy");
			});

			app.UseStatusCodePages(async context =>
			{
				context.HttpContext.Response.ContentType = "application/json";
				var response = string.Empty;

				if (context.HttpContext.Response.StatusCode == 404)
				{
					response = JsonConvert.SerializeObject(new ErrorResponse("404 - Page not found."));
				}

				await context.HttpContext.Response.WriteAsync(response).ConfigureAwait(false);
			});
		}
	}
}
