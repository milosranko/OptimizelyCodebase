namespace Optimizely.Demo.PublicWeb.Models.ViewModels;

public record ArticleCardViewModel
{
    public string Heading { get; init; }
    public string LeadText { get; init; }
    public XhtmlString MainBody { get; init; }
    public string ImageUrl { get; init; }
}
