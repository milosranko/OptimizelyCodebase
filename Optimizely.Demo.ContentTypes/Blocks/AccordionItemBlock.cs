﻿using EPiServer.Authorization;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web;
using Optimizely.Demo.ContentTypes.Models.Blocks.Base;
using Optimizely.Demo.Core.Models.Blocks.Local;
using System.ComponentModel.DataAnnotations;

namespace Optimizely.Demo.ContentTypes.Blocks;

[ContentType(GUID = "{57945FA3-5A12-40D1-BA61-5267F41FBB69}")]
[Access(Roles = $"{Roles.Administrators}")]
public class AccordionItemBlock : BlockBase
{
	#region Content tab

	[CultureSpecific]
	[Display(
		GroupName = "Content",
		Order = 100)]
	public virtual string Heading { get; set; }

	[CultureSpecific]
	[Display(
		GroupName = "Content",
		Order = 110)]
	[UIHint(UIHint.Textarea, PresentationLayer.Edit)]
	public virtual string LeadText { get; set; }

	[Display(
		GroupName = "Content",
		Order = 120)]
	public virtual LinkBlock LinkButton { get; set; }

	[CultureSpecific]
	[Display(
		GroupName = "Content",
		Order = 130)]
	[UIHint(UIHint.Image)]
	public virtual ContentReference Image { get; set; }

	#endregion
}
