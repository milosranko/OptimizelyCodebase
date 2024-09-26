using EPiServer.Shell;
using EPiServer.Shell.ViewComposition;

namespace Optimizely.Demo.Core.Gadgets;

[Component]
////PlugInAreas = "/episerver/cms/assets",
//Categories = "cms",
//AllowedRoles = "WebEditors,CmsAdmin,WebAdmins,Administrators",
////WidgetType = "/ClientResources/TestGadget.js",
////Define language path to translate Title/Description.
////LanguagePath = "/customtranslations/components/customcomponent";
////SortOrder = 200,
//Title = "My gadget",
//Description = "A custom component that shows information about the current content item.")]
public class TestGadget : ComponentDefinitionBase
{
    public TestGadget() : base("/ClientResources/TestGadget.js", "My gadget", "A custom component that shows information about the current content item.")
    {
        IsAvailableForUserSelection = true;
        SortOrder = 200;
        Categories = ["cms"];
        PlugInAreas =
        [
            PlugInArea.Assets
        ];
    }
}
