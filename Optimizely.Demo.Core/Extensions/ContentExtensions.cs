using EPiServer.Core;
using EPiServer.Filters;
using EPiServer.Framework.Web;
using EPiServer.ServiceLocation;

namespace Optimizely.Demo.ContentTypes.Extensions;

public static class ContentExtensions
{
    public static IEnumerable<T> FilterForDisplay<T>(
        this IEnumerable<T> contents,
        bool requirePageTemplate = false,
        bool requireVisibleInMenu = false) where T : IContent
    {
        var accessFilter = new FilterAccess();
        var publishedFilter = new FilterPublished();

        contents = contents.Where(x => !publishedFilter.ShouldFilter(x) && !accessFilter.ShouldFilter(x));

        if (requirePageTemplate)
        {
            var templateFilter = ServiceLocator.Current.GetInstance<FilterTemplate>();
            templateFilter.TemplateTypeCategories = TemplateTypeCategories.Request;
            contents = contents.Where(x => !templateFilter.ShouldFilter(x));
        }

        if (requireVisibleInMenu)
        {
            contents = contents.Where(x => x.VisibleInMenu());
        }

        return contents;
    }

    public static bool VisibleInMenu(this IContent content)
    {
        if (content is not PageData page)
        {
            return true;
        }

        return page.VisibleInMenu;
    }
}
