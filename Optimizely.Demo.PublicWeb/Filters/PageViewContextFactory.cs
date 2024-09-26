using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Optimizely.Demo.ContentTypes.Pages;
using Optimizely.Demo.Core.Models.Pages;
using Optimizely.Demo.Core.Models.Pages.Base;
using Optimizely.Demo.Core.Models.ViewModels;
using Optimizely.Demo.Core.Services.SiteSettings;
using Optimizely.Demo.PublicWeb.Models;

namespace Optimizely.Demo.Core.Business;

[ServiceConfiguration]
public class PageViewContextFactory
{
    private readonly ISiteSettingsManager _siteSettingsManager;

    public PageViewContextFactory(ISiteSettingsManager siteSettingsManager)
    {
        _siteSettingsManager = siteSettingsManager;
    }

    public LayoutModel CreateLayout(PageBase page)
    {
        var siteSettings = _siteSettingsManager.CurrentSiteSettings<SiteSettingsPage, SiteSettingsModel>();
        var siteName = siteSettings?.SiteName;

        return new LayoutModel
        {
            MetaData = CreateMetaData(page),
            OpenGraph = CreateOpenGraph(page, siteName),
            Header = CreateHeader(),
            Footer = CreateFooter(),
            PageTitle = page is StartPageBase ? siteName : $"{page.PageName} | {siteName}",
            SiteName = siteName,
            BodyScripts = siteSettings?.BodyScripts,
            HeaderStyles = siteSettings?.HeaderStyles,
            HeaderScripts = siteSettings?.HeaderScripts,
            FooterScripts = siteSettings?.FooterScripts
        };
    }

    private HeaderModelBase CreateHeader()
    {
        return new HeaderModelBase
        {

        };
    }

    private MetaDataModel CreateMetaData(PageBase page)
    {
        if (page is not PageBaseSeo sitePage)
            return new MetaDataModel();

        return new MetaDataModel
        {
            Description = sitePage.MetaDescription,
            NoRobots = sitePage.MetaNoRobots
        };
    }

    private OpenGraphModel CreateOpenGraph(PageBase page, string? siteName)
    {
        if (page is not PageBaseSeo seoPage)
            return new OpenGraphModel();

        var imageUrl = default(string?);

        if (!ContentReference.IsNullOrEmpty(seoPage.OpenGraphImage))
        {
            string url = UrlResolver.Current.GetUrl(seoPage.OpenGraphImage);
            imageUrl = UriSupport.AbsoluteUrlBySettings(url) + "?w=1200";
        }

        return new OpenGraphModel
        {
            ImageUrl = imageUrl,
            PageUrl = UrlResolver.Current.GetUrl(seoPage.ContentLink, null, new VirtualPathArguments { ForceAbsolute = true }),
            Title = seoPage is StartPage && !string.IsNullOrEmpty(siteName) ? siteName : seoPage.Name,
            Description = seoPage.OpenGraphDescription
        };
    }

    private FooterModelBase CreateFooter()
    {
        return new FooterModelBase
        {
            //Column1 = startPage.Column1,
            //Column2 = startPage.Column2,
            //Column3 = startPage.Column3
        };
    }
}
