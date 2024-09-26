using Optimizely.Demo.Core.Models.Definitions;

namespace Optimizely.Demo.PublicWeb.Models;

public record SiteSettingsModel
{
    public string? SiteName { get; set; }
    public IEnumerable<NameValueStringDefinition>? HeaderStyles { get; set; }
    public IEnumerable<NameValueStringDefinition>? HeaderScripts { get; set; }
    public IEnumerable<NameValueStringDefinition>? BodyScripts { get; set; }
    public IEnumerable<NameValueStringDefinition>? FooterStyles { get; set; }
    public IEnumerable<NameValueStringDefinition>? FooterScripts { get; set; }
}
