using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web;
using Optimizely.Demo.ContentTypes.Attributes.Validations;
using Optimizely.Demo.ContentTypes.Blocks;
using Optimizely.Demo.ContentTypes.Constants;
using Optimizely.Demo.ContentTypes.Models.Pages.Base;
using System.ComponentModel.DataAnnotations;

namespace Optimizely.Demo.ContentTypes.Pages;

[ContentType(
    GUID = "{69011830-6192-4EE5-A137-5FD4CE16919F}",
    GroupName = Globals.GroupNames.Default)]
[AvailableContentTypes(
    Availability.None)]
public class EventPage : PageBaseSeo
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

    [CultureSpecific]
    [Display(
        GroupName = SystemTabNames.Content,
        Order = 40)]
    [AllowedTypesForXhtml(new[] { typeof(TeaserBlock) })]
    public virtual XhtmlString MainContent { get; set; }

    [CultureSpecific]
    [Display(
        GroupName = SystemTabNames.Content,
        Order = 50)]
    [AllowedTypes(new[] {
            typeof(TeaserBlock)
        })]
    public virtual ContentArea MainContentArea { get; set; }

    #endregion
}
