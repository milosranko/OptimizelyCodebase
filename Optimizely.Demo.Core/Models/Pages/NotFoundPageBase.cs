using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Optimizely.Demo.ContentTypes.Attributes;
using Optimizely.Demo.ContentTypes.Models.Pages.Base;

namespace Optimizely.Demo.ContentTypes.Models.Pages;

[AvailableContentTypes(Availability.None)]
[SiteImageUrl("NotFoundPage.png")]
public abstract class NotFoundPageBase : PageBasePublic
{
    #region Public properties

    public override void SetDefaultValues(ContentType contentType)
    {
        base.SetDefaultValues(contentType);

        VisibleInMenu = false;
    }

    #endregion
}
