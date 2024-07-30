using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Optimizely.Demo.ContentTypes.Constants;
using Optimizely.Demo.ContentTypes.Models.Pages;
using System.ComponentModel.DataAnnotations;

namespace Optimizely.Demo.ContentTypes.Pages;

[ContentType(
	GUID = "{005DBD8E-B8C6-4C0F-8066-44CFE01FD535}",
	GroupName = Globals.GroupNames.Specialized)]
public class NotFoundPage : NotFoundPageBase
{
	#region Content tab

	[CultureSpecific]
	[Display(
		GroupName = SystemTabNames.Content,
		Order = 100)]
	public virtual string Heading { get; set; }

	#endregion
}
