using EPiServer;
using EPiServer.Core;
using EPiServer.Web;
using Mapster;
using Optimizely.Demo.Core.Business.Caching;
using Optimizely.Demo.Core.Models.Pages;

namespace Optimizely.Demo.Core.Services.SiteSettings;

public class SiteSettingsManager : ISiteSettingsManager
{
    private readonly IContentLoader _contentLoader;
    private readonly ICacheService _cacheService;
    private const string CacheKeyPrefix = "SiteSettings:";

    public SiteSettingsManager(IContentLoader contentLoader, ICacheService cacheService)
    {
        _contentLoader = contentLoader;
        _cacheService = cacheService;
    }

    public TOutputModel CurrentSiteSettings<TSiteSettingsModel, TOutputModel>()
        where TSiteSettingsModel : SiteSettingsPageBase
        where TOutputModel : class, new()
    {
        var cacheKey = $"{CacheKeyPrefix}{SiteDefinition.Current.StartPage.ID}";
        return _cacheService.GetOrSet(cacheKey, () => GetSiteSettings<TSiteSettingsModel, TOutputModel>(SiteDefinition.Current.StartPage));
    }

    public T? SpecificSiteSettings<T>(ContentReference startPageRef) where T : SiteSettingsPageBase
    {
        if (_contentLoader.TryGet<StartPageBase>(startPageRef, out var startPage))
            return _contentLoader.GetChildren<T>(startPage.ContentLink).SingleOrDefault();

        return default;
    }

    public void RemoveFromCache(int contentId)
    {
        _cacheService.Remove($"{CacheKeyPrefix}{contentId}");
    }

    private TOutputModel GetSiteSettings<TSiteSettingsModel, TOutputModel>(ContentReference siteStartPageRef)
        where TSiteSettingsModel : SiteSettingsPageBase
        where TOutputModel : class, new()
    {
        if (_contentLoader.TryGet<StartPageBase>(siteStartPageRef, out var startPage))
            return _contentLoader
                .GetChildren<TSiteSettingsModel>(startPage.ContentLink)
                .SingleOrDefault()
                .Adapt<TOutputModel>();

        return new TOutputModel();
    }
}
