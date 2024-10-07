using Optimizely.Demo.Core.Models.Definitions;

namespace Optimizely.Demo.PublicWeb.Models;

public record SiteSettingsModel
{
    public string? SiteName { get; init; }
    public IEnumerable<NameValueStringDefinition>? HeaderStyles { get; init; }
    public IEnumerable<NameValueStringDefinition>? HeaderScripts { get; init; }
    public IEnumerable<NameValueStringDefinition>? BodyScripts { get; init; }
    public IEnumerable<NameValueStringDefinition>? FooterStyles { get; init; }
    public IEnumerable<NameValueStringDefinition>? FooterScripts { get; init; }
}
