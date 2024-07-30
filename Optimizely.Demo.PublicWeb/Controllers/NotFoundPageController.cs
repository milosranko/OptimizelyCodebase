using Microsoft.AspNetCore.Mvc;
using Optimizely.Demo.ContentTypes.Helpers;
using Optimizely.Demo.ContentTypes.Pages;
using Optimizely.Demo.Core.Controllers;
using Optimizely.Demo.Core.Models.ViewModels;
using System.Globalization;
using System.Net;

namespace Optimizely.Demo.PublicWeb.Controllers;

[ResponseCache(Duration = 3600)]
public class NotFoundPageController : PageControllerBase<NotFoundPage>
{
	private readonly IContentLoader _contentLoader;

	public NotFoundPageController(IContentLoader contentLoader)
	{
		_contentLoader = contentLoader;
	}

	public ActionResult Index(NotFoundPage currentPage)
	{
		if (!PageHelpers.IsInEditMode())
		{
			Response.StatusCode = (int)HttpStatusCode.NotFound;
			// get the language from the URL by splitting it on '/' character
			var lang = Request.Path.Value?.Split('/');

			if (lang != null && lang.Length > 2)//if it has language prefix in the URL it will have at least 2 members of the split array. second member is a good candidate for language
			{
				//get all cultures and from that list get the one that matches current language
				var cultureInfo = CultureInfo.GetCultures(CultureTypes.AllCultures).FirstOrDefault(x => x.Name == lang[1]);
				var alternativeErrorPage = cultureInfo != null ? _contentLoader.Get<NotFoundPage>(currentPage.ContentLink, new LanguageSelector(cultureInfo.Name)) : null;
				currentPage = alternativeErrorPage ?? currentPage;//if language was present and 404 page exists in current language continue to display that page else display fallback page
			}
		}

		var model = PageViewModel.Create(currentPage);

		return View(model);
	}
}
