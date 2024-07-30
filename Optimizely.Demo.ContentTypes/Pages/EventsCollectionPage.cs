using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web;
using Optimizely.Demo.ContentTypes.Constants;
using Optimizely.Demo.ContentTypes.Models.Pages.Base;
using System.ComponentModel.DataAnnotations;

namespace Optimizely.Demo.ContentTypes.Pages;

[ContentType(
    GUID = "{2A1D74A6-ECC9-4035-8267-735AD0554A88}",
    GroupName = Globals.GroupNames.Default)]
[AvailableContentTypes(
    Availability.Specific,
    Include =
    [
        typeof(YearContainerPage)
    ])]
public class EventsCollectionPage : PageBaseSeo
{
    #region Content tab

    [CultureSpecific]
    [Display(
        GroupName = SystemTabNames.Content,
        Order = 10)]
    public virtual string Heading { get; set; }

    [CultureSpecific]
    [Display(
        GroupName = SystemTabNames.Content,
        Order = 20)]
    [UIHint(UIHint.Image)]
    public virtual ContentReference TopImage { get; set; }

    [CultureSpecific]
    [Display(
    GroupName = SystemTabNames.Content,
        Order = 30)]
    [UIHint(UIHint.Textarea, PresentationLayer.Edit)]
    public virtual string LeadText { get; set; }

    #endregion
}
