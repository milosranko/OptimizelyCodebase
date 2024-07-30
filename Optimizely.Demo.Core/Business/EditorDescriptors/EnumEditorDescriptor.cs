using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;

namespace Optimizely.Demo.Core.Business.EditorDescriptors;

public class EnumEditorDescriptor<TEnum> : EditorDescriptor
{
    public override void ModifyMetadata(
        ExtendedMetadata metadata,
        IEnumerable<Attribute> attributes)
    {
        SelectionFactoryType = typeof(EnumSelectionFactory<TEnum>);
        ClientEditingClass = "epi-cms/contentediting/editors/SelectionEditor";

        base.ModifyMetadata(metadata, attributes);
    }
}
