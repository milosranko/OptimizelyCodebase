using EPiServer.Core;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using Optimizely.Demo.ContentTypes.Constants;

namespace Optimizely.Demo.Core.Business.EditorDescriptors;

[EditorDescriptorRegistration(
        TargetType = typeof(XhtmlString),
        EditorDescriptorBehavior = EditorDescriptorBehavior.ExtendBase,
        UIHint = Globals.SiteUIHints.DisableBlocksInsideXhtml)]
public class XhtmlEditorDescriptor : EditorDescriptor
{
    public XhtmlEditorDescriptor()
    {
        AllowedTypes = new[] { typeof(PageData) };
    }
}
