using System.Reflection;
using Microsoft.OpenApi.Models;

namespace RecImage.Api.DependencyInjections;

internal static class SwaggerDependencyInjections
{
    public static IServiceCollection AddRecImageSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = "RecImage.Test.Api",
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

        return services;
    }

    public static IApplicationBuilder AddRecImageSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger();

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "RecImage.Test.Api");
            c.RoutePrefix = string.Empty;

            c.OAuthClientId("swagger-ui");
            c.OAuthClientSecret("swagger-ui-secret");
            c.OAuthRealm("swagger-ui-realm");
            c.OAuthAppName("Swagger UI");
        });

        return app;
    }
}