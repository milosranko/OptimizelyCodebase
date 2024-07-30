using EPiServer.Cms.Shell.UI.ObjectEditing;
using EPiServer.Core;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using Optimizely.Demo.ContentTypes.Models.Blocks.Base;
using Optimizely.Demo.ContentTypes.Models.Pages;

namespace Optimizely.Demo.Core.Business.EditorDescriptors;

[EditorDescriptorRegistration(TargetType = typeof(CategoryList))]
public class CategoryEditorDescriptor : EditorDescriptor
{
    public override void ModifyMetadata(
        ExtendedMetadata metadata,
        IEnumerable<Attribute> attributes)
    {
        //Use it to hide Category selector on page/block
        var ownerContent = ((ContentDataMetadata)metadata).OwnerContent;
        if (ownerContent is StartPageBase ||
            ownerContent is BlockBase ||
            ownerContent is MediaData)
        {
            metadata.ShowForEdit = false;
        }
    }
}
