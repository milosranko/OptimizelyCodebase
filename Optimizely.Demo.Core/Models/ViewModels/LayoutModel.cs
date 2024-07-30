namespace Optimizely.Demo.Core.Models.ViewModels;

public record LayoutModel
{
	public MetaDataModel? MetaData { get; set; }
	public OpenGraphModel? OpenGraph { get; set; }
	public HeaderModelBase? Header { get; set; }
	public FooterModelBase? Footer { get; set; }
	public string? PageTitle { get; set; }
	public string? SiteName { get; set; }
}
