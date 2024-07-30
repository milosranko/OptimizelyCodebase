using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Optimizely.Demo.ContentTypes.Models.Pages;
using Optimizely.Demo.ContentTypes.Models.Pages.Base;
using Optimizely.Demo.ContentTypes.Pages;
using Optimizely.Demo.Core.Models.ViewModels;

namespace Optimizely.Demo.Core.Business;

[ServiceConfiguration]
public class PageViewContextFactory
{
    public LayoutModel CreateLayout(PageBase page)
    {
        return new LayoutModel
        {
            MetaData = CreateMetaData(page),
            OpenGraph = CreateOpenGraph(page, "siteName"),
            Header = CreateHeader(),
            Footer = CreateFooter(),
            PageTitle = page is StartPageBase ? "siteName" : page.PageName + " | " + "siteName",
            SiteName = "siteName"
        };
    }

    private HeaderModelBase CreateHeader()
    {
        return new HeaderModelBase
        {
            //LogoLink = 
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

    private OpenGraphModel CreateOpenGraph(PageBase page, string siteName)
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
            Title = seoPage is StartPage ? siteName : seoPage.Name,
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
