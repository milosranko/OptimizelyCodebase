using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using Optimizely.Demo.ContentTypes.Constants;

namespace Optimizely.Demo.Core.Business.EditorDescriptors;

[EditorDescriptorRegistration(TargetType = typeof(String[]), UIHint = Globals.SiteUIHints.StringList)]
public class StringListEditorDescriptor : EditorDescriptor
{
    public override void ModifyMetadata(
        ExtendedMetadata metadata,
        IEnumerable<Attribute> attributes)
    {
        ClientEditingClass = "alloy/editors/StringList";

        base.ModifyMetadata(metadata, attributes);
    }
}
