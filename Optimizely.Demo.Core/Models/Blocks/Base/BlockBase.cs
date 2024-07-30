using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;

namespace Optimizely.Demo.ContentTypes.Models.Blocks.Base;

public abstract class BlockBase : BlockData
{
    private Lazy<PageData> _currentPage = new(ServiceLocator.Current.GetInstance<IPageRouteHelper>().Page);

    public PageData? CurrentPage
    {
        get
        {
            try
            {
                return _currentPage.Value;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
