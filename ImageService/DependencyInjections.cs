using ImageService.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ImageService;

public static class DependencyInjections
{
	public static IServiceCollection AddImageServices(this IServiceCollection collection)
	{
		collection.AddScoped<IImageConverter, ImageConverter>();

		return collection;
	}
}