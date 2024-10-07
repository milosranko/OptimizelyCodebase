namespace Optimizely.Demo.PublicWeb.Models.ViewModels;

public record ModalViewModel
{
    public Type Caller { get; init; }
    public string Heading { get; init; }
    public string LeadText { get; init; }
    public XhtmlString MainBody { get; init; }
    public ContentReference Image { get; init; }
}
