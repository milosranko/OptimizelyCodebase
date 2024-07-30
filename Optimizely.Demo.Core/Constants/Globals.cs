using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Optimizely.Demo.ContentTypes.Constants;

public class Globals
{
    [GroupDefinitions]
    public static class GroupNames
    {
        [Display(Name = "Default", Order = 10)]
        public const string Default = "Default";

        [Display(Name = "Metadata", Order = 20)]
        public const string MetaData = "Metadata";

        [Display(Name = "Footer", Order = 30)]
        public const string Footer = "Footer";

        [Display(Name = "Aside content", Order = 40)]
        public const string AsideContent = "Aside content";

        [Display(Name = "Site Settings", Order = 50)]
        public const string SiteSettings = "Site Settings";

        [Display(Name = "SEO", Order = 60)]
        public const string SEO = "SEO";

        [Display(Name = "Specialized", Order = 70)]
        public const string Specialized = "Specialized";
    }

    public static class SiteUIHints
    {
        public const string StringList = "StringList";
        public const string StringsCollection = "StringsCollection";
        public const string DisableBlocksInsideXhtml = "DisableBlocksInsideXhtml";
        public const string WideTextArea = "WideTextArea";
    }

    public static class ContentAreaTags
    {
        public const string FullWidth = "full";
        public const string WideWidth = "wide";
        public const string HalfWidth = "half";
        public const string NarrowWidth = "narrow";
        public const string NoRenderer = "norenderer";
    }
}
