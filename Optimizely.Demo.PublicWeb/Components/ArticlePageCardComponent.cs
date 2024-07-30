using Microsoft.AspNetCore.Mvc;
using Optimizely.Demo.ContentTypes.Pages;
using Optimizely.Demo.Core.Components;
using Optimizely.Demo.PublicWeb.Models.ViewModels;

namespace Optimizely.Demo.PublicWeb.Components;

public class ArticlePageCardComponent : PartialContentComponentBase<ArticlePage>
{
	protected override IViewComponentResult InvokeComponent(ArticlePage currentContent)
	{
		var model = new ModalViewModel
		{
			Caller = typeof(ArticlePage),
			Heading = currentContent.Heading,
			LeadText = currentContent.LeadText,
			MainBody = currentContent.MainContent,
			Image = currentContent.TopImage
		};

		return View("~/Views/Shared/Partials/Modal.cshtml", model);
		//return View("ArticleCard", model);
	}
}
