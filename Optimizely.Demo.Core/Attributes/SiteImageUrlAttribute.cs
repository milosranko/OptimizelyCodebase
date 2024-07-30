using EPiServer.DataAnnotations;

namespace Optimizely.Demo.ContentTypes.Attributes;

public class SiteImageUrlAttribute : ImageUrlAttribute
{
    public SiteImageUrlAttribute(string path) : base("~/img/thumbs/" + path)
    { }
}
