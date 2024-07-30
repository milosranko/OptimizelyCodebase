using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Optimizely.Demo.ContentTypes.Constants;
using Optimizely.Demo.ContentTypes.Models.Pages.Base;

namespace Optimizely.Demo.ContentTypes.Pages;

[ContentType(
    GUID = "{11A2FDA5-A4D3-48B7-8835-CCB9850C5D33}",
    GroupName = Globals.GroupNames.Specialized)]
[AvailableContentTypes(
    Availability.Specific,
    Include =
    [
        typeof(MonthContainerPage)
    ])]
public class YearContainerPage : ContainerBase
{
}
