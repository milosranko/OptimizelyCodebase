namespace Optimizely.Demo.PublicWeb.Models.ViewModels;

public record ModalViewModel
{
	public Type Caller { get; set; }
	public string Heading { get; set; }
	public string LeadText { get; set; }
	public XhtmlString MainBody { get; set; }
	public ContentReference Image { get; set; }
}
