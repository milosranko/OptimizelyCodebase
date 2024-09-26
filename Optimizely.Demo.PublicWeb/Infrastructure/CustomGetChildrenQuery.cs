using EPiServer.Cms.Shell.UI.Rest.ContentQuery;
using EPiServer.Filters;
using EPiServer.Logging;
using EPiServer.Security;
using EPiServer.Shell.Web.Internal;
using EPiServer.Web;

namespace Optimizely.Demo.PublicWeb.Infrastructure;

public class CustomGetChildrenQuery : GetChildrenQuery
{
    public EPiServer.Logging.ILogger Logger = LogManager.GetLogger(typeof(CustomGetChildrenQuery));

    public CustomGetChildrenQuery(
        IContentQueryHelper queryHelper,
        IContentRepository contentRepository,
        LanguageSelectorFactory languageSelectorFactory,
        UserInterfaceOptions userInterfaceOptions)
        : base(queryHelper, contentRepository, languageSelectorFactory, userInterfaceOptions)
    { }

    protected override IEnumerable<IContent> GetContent(ContentQueryParameters parameters)
    {
        try
        {
            var content = base.GetContent(parameters);

            if (content != null)
            {
                content = content.Where(x =>
                {
                    return x.ContentLink.CompareToIgnoreWorkID(SiteDefinition.Current.RootPage) ||
                            x.ContentLink.CompareToIgnoreWorkID(SiteDefinition.Current.SiteAssetsRoot) ||
                            FilterAccess.QueryDistinctAccessEdit(x, AccessLevel.Edit);
                });
            }

            return [];
        }
        catch (Exception ex)
        {
            Logger.Log(Level.Critical, ex.Message);
        }

        return [];
    }
}
