using Optimizely.Demo.PublicWeb.Filters;

namespace Optimizely.Demo.PublicWeb.Extensions;

public static class AppBuilderExtensions
{
	public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app, bool isDevelopment, string errorHandlingPath)
	{
		if (isDevelopment)
		{
			app.UseDeveloperExceptionPage();
		}
		else
		{
			app.UseMiddleware<ExceptionFilter>(errorHandlingPath);
			app.UseHsts();
		}

		return app;
	}
}
