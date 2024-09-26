using Optimizely.Demo.Core.Models.Pages.Interfaces;

namespace Optimizely.Demo.Core.Models.Pages.Base;

public abstract class PageBasePublic : PageBase, ISearchable
{
    #region Public properties

    public virtual string BodyCss => string.Empty;

    public virtual bool ExcludeFromSiteSearchResults { get; set; }

    #endregion
}
