using EPiServer.Core;
using Optimizely.Demo.Core.Models.Pages;

namespace Optimizely.Demo.Core.Services.SiteSettings;

public interface ISiteSettingsManager
{
    TOutputModel CurrentSiteSettings<TSiteSettingsModel, TOutputModel>()
        where TSiteSettingsModel : SiteSettingsPageBase
        where TOutputModel : class, new();

    T? SpecificSiteSettings<T>(ContentReference startPageRef)
        where T : SiteSettingsPageBase;

    void RemoveFromCache(int contentId);
}
