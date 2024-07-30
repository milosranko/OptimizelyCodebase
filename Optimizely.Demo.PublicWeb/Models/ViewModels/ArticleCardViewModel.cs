namespace Optimizely.Demo.PublicWeb.Models.ViewModels;

public record ArticleCardViewModel
{
	public string Heading { get; set; }
	public string LeadText { get; set; }
	public XhtmlString MainBody { get; set; }
	public string ImageUrl { get; set; }
}
