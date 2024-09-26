using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Optimizely.Demo.ContentTypes.Constants;
using Optimizely.Demo.Core.Models.Pages.Base;

namespace Optimizely.Demo.ContentTypes.Pages;

[ContentType(
    GUID = "{B86A1A0D-1091-4E7D-9FD8-F0DF20C0BF0D}",
    GroupName = Globals.TabNames.Specialized)]
[AvailableContentTypes(
    Availability.Specific,
    Include =
    [
        typeof(EventPage)
    ])]
public class MonthContainerPage : ContainerBase
{
}
