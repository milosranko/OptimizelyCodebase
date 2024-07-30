using EPiServer.Authorization;
using EPiServer.Cms.Shell.UI.ObjectEditing;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Personalization.VisitorGroups;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web;
using Optimizely.Demo.ContentTypes.Attributes;
using Optimizely.Demo.ContentTypes.Models.Blocks.Base;
using Optimizely.Demo.Core.Business.EditorDescriptors;
using Optimizely.Demo.Core.Models.Blocks.Local;
using System.ComponentModel.DataAnnotations;

namespace Optimizely.Demo.ContentTypes.Blocks;

[Access(Roles = $"{Roles.WebAdmins}")]
[ContentType(GUID = "{C98C99EA-A630-49CD-8A45-5AEF47EE265D}")]
[SiteImageUrl("TeaserBlock.png")]
//[InlineBlockEditSettings(ShowNameProperty = true)]
public class TeaserBlock : BlockBase
{
    #region Content tab

    [ReloadOnChange]
    [CultureSpecific]
    [Display(
        GroupName = SystemTabNames.Content,
        Order = 100)]
    [StringLength(50)]
    public virtual string Heading { get; set; }

    [CultureSpecific]
    [Display(
        GroupName = SystemTabNames.Content,
        Order = 110)]
    [UIHint(UIHint.Textarea, PresentationLayer.Edit)]
    public virtual string LeadText { get; set; }

    [Display(
        GroupName = SystemTabNames.Content,
        Order = 120)]
    public virtual LinkBlock LinkButton { get; set; }

    [CultureSpecific]
    [Display(
        GroupName = SystemTabNames.Content,
        Order = 130)]
    [UIHint(UIHint.Image)]
    public virtual ContentReference Image { get; set; }

    [Display(
        Name = "Form Fields",
        GroupName = SystemTabNames.Content,
        Order = 140)]
    public virtual IList<FormFieldBlock> FormFields { get; set; }

    [Display(
        Name = "Questionnaire Step 1",
        GroupName = SystemTabNames.Content,
        Order = 150)]
    public virtual QuestionnaireBlock QuestionnaireBlock1 { get; set; }

    [Display(
        Name = "Questionnaire Step 2",
        GroupName = SystemTabNames.Content,
        Order = 160)]
    public virtual QuestionnaireBlock QuestionnaireBlock2 { get; set; }

    [Display(
        Name = "Questionnaire Step 3",
        GroupName = SystemTabNames.Content,
        Order = 170)]
    public virtual QuestionnaireBlock QuestionnaireBlock3 { get; set; }

    #endregion
}

[ContentType(
    DisplayName = "Form Field Block",
    AvailableInEditMode = false,
    GUID = "{CF34DA3B-E85A-4989-8350-EDB25F4D5157}")]
public class FormFieldBlock : BlockData
{
    [Display(
        GroupName = SystemTabNames.Content,
        Order = 10)]
    public virtual string Name { get; set; }

    [Display(
        GroupName = SystemTabNames.Content,
        Order = 20)]
    public virtual string Label { get; set; }

    [Display(
        GroupName = SystemTabNames.Content,
        Order = 30)]
    public virtual bool Required { get; set; }

    [Display(
        GroupName = SystemTabNames.Content,
        Order = 40)]
    [EditorDescriptor(EditorDescriptorType = typeof(EnumEditorDescriptor<FieldType>))]
    public virtual FieldType Type { get; set; }
}

[ContentType(
    DisplayName = "Questionnaire Block",
    AvailableInEditMode = false,
    GUID = "{129E55F1-3257-47B7-87BE-BC06FF520F7F}")]
public class QuestionnaireBlock : BlockData
{
    [Display(
        GroupName = SystemTabNames.Content,
        Order = 10)]
    public virtual string Text { get; set; }

    [Display(
        Name = "Allow multiple answers",
        GroupName = SystemTabNames.Content,
        Order = 15)]
    public virtual bool Multiple { get; set; }

    [Display(
        GroupName = SystemTabNames.Content,
        Order = 20)]
    [UIHint(UIHint.Image)]
    public virtual ContentReference Image { get; set; }

    [Display(
        GroupName = SystemTabNames.Content,
        Order = 30)]
    public virtual IList<QuestionnaireAnswerBlock> Answers { get; set; }

    [Display(
        GroupName = SystemTabNames.Content,
        Order = 40)]
    public virtual string ButtonText { get; set; }
}

[ContentType(
    DisplayName = "Questionnaire Answer Block",
    AvailableInEditMode = false,
    GUID = "{F34932D3-6AA6-4957-9BBF-0461E246EFCF}")]
public class QuestionnaireAnswerBlock : BlockData
{
    [Display(
        GroupName = SystemTabNames.Content,
        Order = 10)]
    [AutoSuggestSelection(typeof(VisitorGroupsSelectionQuery))]
    public virtual string VisitorGroup { get; set; }

    [Display(
        GroupName = SystemTabNames.Content,
        Order = 20)]
    public virtual string Label { get; set; }
}

[ServiceConfiguration(typeof(ISelectionQuery))]
public class VisitorGroupsSelectionQuery : ISelectionQuery
{
    private readonly IVisitorGroupRepository _visitorGroupRepository;
    IEnumerable<SelectItem> _items = [];

    public VisitorGroupsSelectionQuery(IVisitorGroupRepository visitorGroupRepository)
    {
        _visitorGroupRepository = visitorGroupRepository;
        _items = _visitorGroupRepository
            .List()
            .Select(x => new SelectItem
            {
                Text = x.Name,
                Value = x.Id
            });
    }


    public ISelectItem GetItemByValue(string value)
    {
        return _items.FirstOrDefault(i => i.Value.ToString().Equals(value));
    }

    public IEnumerable<ISelectItem> GetItems(string query)
    {
        return _items.Where(i => i.Text.StartsWith(query, StringComparison.OrdinalIgnoreCase));
    }
}

public enum FieldType
{
    Text,
    Email,
    Phone,
    Password,
    Checkbox
}
