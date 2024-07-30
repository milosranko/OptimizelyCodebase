using EPiServer.Cms.TinyMce.Core;
using EPiServer.ServiceLocation;
using Microsoft.Extensions.DependencyInjection;

namespace Optimizely.Demo.Core.Business.Initialization;

public static class DefaultTinyMceInitialization
{
    public static IServiceCollection AddCustomTinyMceConfiguration(this IServiceCollection services)
    {
        services.Configure<TinyMceConfiguration>(config =>
        {
            config.Default()
                .DisableMenubar()
                .AddEpiserverSupport()
                .AddPlugin("help image fullscreen lists searchreplace anchor")
                .Height(300)
                .Width(585)
                .Resize(TinyMceResize.Both)
                .BodyClass("custom_body_class")
                //.ContentCss("/static/css/editor.css")
                .Toolbar("formatselect | bold italic | epi-link anchor image epi-image-editor epi-personalized-content | bullist numlist outdent indent | searchreplace fullscreen | help");
        });

        return services;
    }
}
