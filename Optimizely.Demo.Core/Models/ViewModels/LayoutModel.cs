using Optimizely.Demo.Core.Models.Definitions;

namespace Optimizely.Demo.Core.Models.ViewModels;

public record LayoutModel
{
    public MetaDataModel? MetaData { get; set; }
    public OpenGraphModel? OpenGraph { get; set; }
    public HeaderModelBase? Header { get; set; }
    public FooterModelBase? Footer { get; set; }
    public string? PageTitle { get; set; }
    public string? SiteName { get; set; }
    public IEnumerable<NameValueStringDefinition>? BodyScripts { get; set; }
    public IEnumerable<NameValueStringDefinition>? HeaderStyles { get; set; }
    public IEnumerable<NameValueStringDefinition>? HeaderScripts { get; set; }
    public IEnumerable<NameValueStringDefinition>? FooterScripts { get; set; }
}
