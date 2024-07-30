namespace Optimizely.Demo.ImageCache;

public class OptimizelyImageCacheStartupFilter : IStartupFilter
{
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return app =>
        {
            app.UseMiddleware<OptimizelyImageCacheMiddleware>();
            next(app);
        };
    }
}
