using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using Optimizely.Demo.ContentTypes.Constants;
using Optimizely.Demo.Core.Models.Definitions;
using Optimizely.Demo.Core.Models.Pages.Base;
using System.ComponentModel.DataAnnotations;

namespace Optimizely.Demo.Core.Models.Pages;

public abstract class SiteSettingsPageBase : PageBase
{
    #region Content tab

    [Display(
        GroupName = SystemTabNames.Content,
        Order = 100)]
    [CultureSpecific]
    [Required]
    public virtual string SiteName { get; set; }

    [Display(
        GroupName = SystemTabNames.Content,
        Order = 110)]
    [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<NameValueStringDefinition>))]
    public virtual IList<NameValueStringDefinition> BodyScripts { get; set; }

    #endregion

    #region Header tab

    [Display(
        GroupName = Globals.TabNames.Header,
        Order = 200)]
    [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<NameValueStringDefinition>))]
    public virtual IList<NameValueStringDefinition> HeaderScripts { get; set; }

    [Display(
        GroupName = Globals.TabNames.Header,
        Order = 210)]
    [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<NameValueStringDefinition>))]
    public virtual IList<NameValueStringDefinition> HeaderStyles { get; set; }

    #endregion

    #region Footer tab

    [Display(
        GroupName = Globals.TabNames.Footer,
        Order = 300)]
    [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<NameValueStringDefinition>))]
    public virtual IList<NameValueStringDefinition> FooterScripts { get; set; }

    #endregion

    #region Public properties

    public override void SetDefaultValues(ContentType contentType)
    {
        base.SetDefaultValues(contentType);

        VisibleInMenu = false;
    }

    #endregion
}
