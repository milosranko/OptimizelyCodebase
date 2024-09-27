using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Optimizely.Demo.ContentTypes.Constants;
using Optimizely.Demo.Core.Models.Pages;

namespace Optimizely.Demo.ContentTypes.Pages;

[ContentType(
    GUID = "{7A553D60-F2DF-4094-A650-8812F755EF6E}",
    GroupName = Globals.TabNames.Specialized)]
[AvailableContentTypes(
    Availability.None,
    IncludeOn =
    [
        typeof(StartPage)
    ])]
public class SiteSettingsPage : SiteSettingsPageBase
{
    #region Content tab

    #endregion
}
