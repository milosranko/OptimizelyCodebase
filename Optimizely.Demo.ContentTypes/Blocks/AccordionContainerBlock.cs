﻿using EPiServer.Authorization;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using Optimizely.Demo.ContentTypes.Models.Blocks.Base;
using System.ComponentModel.DataAnnotations;

namespace Optimizely.Demo.ContentTypes.Blocks;

[ContentType(DisplayName = "Accordion Container Block", GUID = "{2AB06B13-1082-4FB2-A9E0-BAE99983BEBF}")]
[Access(Roles = $"{Roles.Administrators}")]
public class AccordionContainerBlock : BlockBase
{
	#region Content tab

	[CultureSpecific]
	[Display(
		GroupName = "Heading",
		Order = 100)]
	public virtual string Heading { get; set; }

	[CultureSpecific]
	[Display(
		GroupName = "Items",
		Order = 110)]
	[AllowedTypes(typeof(AccordionItemBlock))]
	public virtual ContentArea Items { get; set; }

	#endregion
}
