using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using Optimizely.Demo.ContentTypes.Constants;
using Optimizely.Demo.Core.Models.Blocks.Local;
using System.ComponentModel.DataAnnotations;

namespace Optimizely.Demo.Core.Models.Pages.Base;

public abstract class PageBaseSeo : PageBasePublic
{
    #region SEO tab

    [Display(
        GroupName = Globals.TabNames.MetaData,
        Order = 7000)]
    [CultureSpecific]
    public virtual string MetaTitle { get; set; }

    [Display(
        GroupName = Globals.TabNames.MetaData,
        Order = 7010)]
    public virtual bool MetaNoRobots { get; set; }

    [Display(
        GroupName = Globals.TabNames.MetaData,
        Order = 7020)]
    [CultureSpecific]
    [UIHint(UIHint.Textarea)]
    public virtual string MetaDescription { get; set; }

    [Display(
        GroupName = Globals.TabNames.MetaData,
        Order = 7030)]
    [UIHint(UIHint.Textarea)]
    [CultureSpecific]
    public virtual string OpenGraphDescription { get; set; }

    [Display(
        GroupName = Globals.TabNames.MetaData,
        Order = 7040)]
    [UIHint(UIHint.Image)]
    public virtual ContentReference OpenGraphImage { get; set; }

    [Display(
        GroupName = Globals.TabNames.SEO,
        Order = 7050)]
    public virtual SitemapSettingsBlock SitemapSettings { get; set; }

    #endregion
}
