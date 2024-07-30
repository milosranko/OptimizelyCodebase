using EPiServer;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Optimizely.Demo.ContentTypes.Constants;
using Optimizely.Demo.ContentTypes.Models.Blocks.Base;
using System.ComponentModel.DataAnnotations;

namespace Optimizely.Demo.Core.Models.Blocks.Local;

[ContentType(
    GUID = "{BBB5D413-D002-4124-B4A5-7770327B93E5}",
    GroupName = Globals.GroupNames.Default,
    AvailableInEditMode = false)]
public class LinkBlock : BlockBase
{
    #region Content tab

    [Display(
        GroupName = SystemTabNames.Content,
        Order = 10)]
    [CultureSpecific]
    public virtual string LinkText { get; set; }

    [Display(
        GroupName = SystemTabNames.Content,
        Order = 20)]
    [CultureSpecific]
    public virtual Url LinkUrl { get; set; }

    #endregion

    #region Public properties

    public bool IsEmpty => string.IsNullOrEmpty(LinkText) && LinkUrl == null;

    #endregion
}
