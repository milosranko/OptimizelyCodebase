using EPiServer.DataAnnotations;

namespace Optimizely.Demo.Core.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class IconTreeContentTypeAttribute : ContentTypeAttribute
{
    public string TreeIcon { get; set; }
}
