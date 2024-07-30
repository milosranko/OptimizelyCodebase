using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Routing;
using Optimizely.Demo.ContentTypes.Models.Pages;

namespace Optimizely.Demo.ContentTypes.Extensions;

public static class PageDataExtensions
{
    public static string CreateUrl(this PageData pd)
    {
        if (pd is StartPageBase && pd.LinkType == PageShortcutType.Shortcut)
        {
            return UrlResolver.Current.GetUrl(UrlResolver.Current.Route(new UrlBuilder(pd.LinkURL)).ContentLink, pd.Language.Name);
        }

        return UrlResolver.Current.GetUrl(pd.ContentLink, pd.Language.Name);
    }
}
