namespace RecImage.Api.DependencyInjections;

internal static class CacheDependencyInjections
{
    public static IApplicationBuilder AddRecImageCache(this IApplicationBuilder app)
    {
        var cacheMaxAgeOneWeek = (60 * 60 * 24 * 7).ToString();

        app.UseStaticFiles(new StaticFileOptions
        {
            OnPrepareResponse = ctx =>
            {
                ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age={cacheMaxAgeOneWeek}");
            }
        });

        return app;
    }
}