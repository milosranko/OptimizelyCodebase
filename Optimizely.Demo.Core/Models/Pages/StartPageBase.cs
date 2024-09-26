using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Optimizely.Demo.ContentTypes.Attributes;
using Optimizely.Demo.Core.Models.Pages.Base;

namespace Optimizely.Demo.Core.Models.Pages;

[SiteImageUrl("start-page.png")]
[AvailableContentTypes(
    Availability.Specific,
    Include =
    [
        typeof(NotFoundPageBase),
        typeof(SiteSettingsPageBase),
        typeof(ContainerBase)
    ])]
public abstract class StartPageBase : PageBaseSeo
{

}
