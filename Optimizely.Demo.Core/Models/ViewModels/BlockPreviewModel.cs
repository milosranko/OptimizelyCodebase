using EPiServer.Core;
using Optimizely.Demo.ContentTypes.Extensions;
using Optimizely.Demo.ContentTypes.Models.Blocks.Base;
using Optimizely.Demo.ContentTypes.Models.Blocks.Interfaces;
using Optimizely.Demo.ContentTypes.Models.Pages.Base;

namespace Optimizely.Demo.Core.Models.ViewModels;

public record BlockPreviewModel : PageViewModel<PageBasePublic>, IPreviewCustomHtml
{
	public ContentArea ContentArea { get; set; }

	public BlockPreviewModel(PageBasePublic page, IContent content) : base(page)
	{
		base.Layout.PageTitle = $"Preview of {content.Name}";
		this.ContentArea = new ContentArea();
		this.ContentArea.Items.Add(new ContentAreaItem
		{
			ContentLink = content.ContentLink
		});

		var block = content.ContentLink.GetBlock<BlockBase>();

		if (block is IPreviewCustomHtml)
		{
			var previewCustomHtml = block as IPreviewCustomHtml;

			FirstLevelTag = previewCustomHtml?.FirstLevelTag;
			FirstLevelCssClass = previewCustomHtml?.FirstLevelCssClass;
			SecondLevelTag = previewCustomHtml?.SecondLevelTag;
			SecondLevelCssClass = previewCustomHtml?.SecondLevelCssClass;
		}
	}

	#region IPreviewCustomHtml properties

	public string? FirstLevelTag { get; }
	public string? FirstLevelCssClass { get; }
	public string? SecondLevelTag { get; }
	public string? SecondLevelCssClass { get; }

	#endregion
}
