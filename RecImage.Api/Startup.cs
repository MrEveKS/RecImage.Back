using Newtonsoft.Json;
using RecImage.Api.Constants;
using RecImage.Api.DependencyInjections;
using RecImage.Business;
using RecImage.Infrastructure.Commons;
using Serilog;

namespace RecImage.Api;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var isDevelop = Environment.GetEnvironmentVariable(EnvironmentConstants.AspnetcoreEnvironment) ==
                        EnvironmentConstants.Development;
        if (isDevelop)
        {
            services.AddRecImageSwagger();
        }

        services
            .AddRecImageCors()
            .AddRecImageCompression()
            .AddBusiness()
            .AddMvcCore(options => { options.RespectBrowserAcceptHeader = true; })
            .AddApiExplorer();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app
                .UseExceptionHandler("/error")
                .UseHsts();
        }

        app.UseResponseCompression();

        if (env.IsDevelopment())
        {
            app.AddRecImageSwagger();
        }

        if (!env.IsDevelopment())
        {
            app.UseHttpsRedirection();
        }

        app
            .AddRecImageCache()
            .UseSerilogRequestLogging()
            .UseRouting()
            .UseCors(env.IsDevelopment() ? CorsConstants.TestPolicy : CorsConstants.ProductionPolicy)
            .UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                    .RequireCors(env.IsDevelopment() ? CorsConstants.TestPolicy : CorsConstants.ProductionPolicy);
            })
            .Map("/error",
                ap => ap.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    var response = JsonConvert
                        .SerializeObject(Result.Failed("Exception", context.Response.StatusCode));

                    await context.Response
                        .WriteAsync(response);
                }))
            .UseStatusCodePages(async context =>
            {
                context.HttpContext.Response.ContentType = "application/json";
                var response = string.Empty;

                if (context.HttpContext.Response.StatusCode == 404)
                {
                    response = JsonConvert
                        .SerializeObject(Result.Failed("Page not found", 404));
                }

                await context.HttpContext.Response
                    .WriteAsync(response);
            });
    }
}