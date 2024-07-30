using Microsoft.AspNetCore.Mvc;
using Optimizely.Demo.ContentTypes.Pages;
using Optimizely.Demo.Core.Controllers;
using Optimizely.Demo.Core.Models.ViewModels;

namespace Optimizely.Demo.PublicWeb.Controllers;

public class StartPageController : PageControllerBase<StartPage>
{
	public IActionResult Index(StartPage currentPage)
	{
		var model = PageViewModel.Create(currentPage);

		return View(model);
	}
}
