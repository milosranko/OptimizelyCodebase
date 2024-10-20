using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Text;

namespace Optimizely.Demo.ImageCache;

public class OptimizelyImageCacheMiddleware
{
    struct ResizeParams
    {
        public bool hasParams;
        public int w;
        public int h;
        public bool autorotate;
        public int quality; // 0 - 100
        public string format; // png, jpg, jpeg
        public string mode; // pad, max, crop, stretch

        public static string[] modes = new string[] { "pad", "max", "crop", "stretch" };

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"w: {w}, ");
            sb.Append($"h: {h}, ");
            sb.Append($"autorotate: {autorotate}, ");
            sb.Append($"quality: {quality}, ");
            sb.Append($"format: {format}, ");
            sb.Append($"mode: {mode}");

            return sb.ToString();
        }
    }

    private readonly ILogger<OptimizelyImageCacheMiddleware> _logger;
    private static readonly string[] suffixes = new[]
    {
        ".png",
        ".jpg",
        ".jpeg"
    };

    private readonly string _imageCachePath = Path.Combine("wwwroot", "uploads");
    private readonly RequestDelegate _next;

    public OptimizelyImageCacheMiddleware(
        RequestDelegate next,
        ILogger<OptimizelyImageCacheMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    private bool IsImagePath(PathString path)
    {
        if (path == null || !path.HasValue)
            return false;

        return suffixes.Any(x => path.Value.EndsWith(x, StringComparison.OrdinalIgnoreCase));
    }

    private ResizeParams GetResizeParams(PathString path, IQueryCollection query)
    {
        var resizeParams = new ResizeParams();

        // before we extract, do a quick check for resize params
        resizeParams.hasParams =
            resizeParams
            .GetType()
            .GetTypeInfo()
            .GetFields()
            .Where(f => f.Name != "hasParams")
            .Any(f => query.ContainsKey(f.Name));

        // if no params present, bug out
        if (!resizeParams.hasParams)
            return resizeParams;

        // extract resize params

        if (query.ContainsKey("format"))
            resizeParams.format = query["format"];
        else
            resizeParams.format = path.Value.Substring(path.Value.LastIndexOf('.') + 1);

        if (query.ContainsKey("autorotate"))
            bool.TryParse(query["autorotate"], out resizeParams.autorotate);

        int quality = 100;
        if (query.ContainsKey("quality"))
            int.TryParse(query["quality"], out quality);
        resizeParams.quality = quality;

        int w = 0;
        if (query.ContainsKey("w"))
            int.TryParse(query["w"], out w);
        resizeParams.w = w;

        int h = 0;
        if (query.ContainsKey("h"))
            int.TryParse(query["h"], out h);
        resizeParams.h = h;

        resizeParams.mode = "max";
        // only apply mode if it's a valid mode and both w and h are specified
        if (h != 0 && w != 0 && query.ContainsKey("mode") && ResizeParams.modes.Any(m => query["mode"] == m))
            resizeParams.mode = query["mode"];

        return resizeParams;
    }

    public async Task Invoke(HttpContext context)
    {
        var path = context.Request.Path;

        // hand to next middleware if we are not dealing with an image
        if (context.Request.Query.Count == 0 || !IsImagePath(path))
        {
            await _next.Invoke(context);
            return;
        }

        // hand to next middleware if we are dealing with an image but it doesn't have any usable resize querystring params
        var resizeParams = GetResizeParams(path, context.Request.Query);
        if (!resizeParams.hasParams || (resizeParams.w == 0 && resizeParams.h == 0))
        {
            await _next.Invoke(context);
            return;
        }

        _logger.LogInformation($"Resizing {path.Value} with params {resizeParams}");

        using var servicesScope = context.RequestServices.CreateScope();
        var appService = servicesScope.ServiceProvider.GetRequiredService<IApplicationService>();
        var imageCacheFullPath = await ProcessImage(appService, api, path.Value, resizeParams);

        await context.Response.SendFileAsync(Path.Combine(Environment.CurrentDirectory, imageCacheFullPath));
        //await _next.Invoke(context);
    }

    private async Task<string> ProcessImage(string path, ResizeParams resizeParams)
    {
        var fileName = Path.GetFileName(path);
        var mediaGuid = Guid.Parse(fileName.Substring(0, Guid.Empty.ToString().Length));
        var media = await api.Media.GetByIdAsync(mediaGuid);

        if (media == null)
            return default;

        var publicUrl = appService.Media.ResizeImage(media, resizeParams.w, resizeParams.h);

        return Path.Combine(_imageCachePath, Path.GetFileName(publicUrl));
    }
}
