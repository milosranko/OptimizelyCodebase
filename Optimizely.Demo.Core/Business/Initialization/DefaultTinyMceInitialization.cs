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
                //Adds block element around text
                //.BlockFormats("Paragraph =p; Header 2=h2;Header 3=h3;Header 4=h4;Header 5=h5;Header 6=h6; Preformatted=pre;")
                //Apply style on selected element
                //.StyleFormats(new { title = "Links", items = new[] { new { title = "Default", classes = "link", selector = "a" }, new { title = "Arrow Link", classes = "arrowLink", selector = "a" }}})
                .Toolbar("formatselect | bold italic | epi-link anchor image epi-image-editor epi-personalized-content | bullist numlist outdent indent | searchreplace fullscreen | help");
        });

        return services;
    }
}
