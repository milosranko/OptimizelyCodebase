using EPiServer.Web.Mvc;
using Optimizely.Demo.Core.Models.Pages.Base;

namespace Optimizely.Demo.Core.Controllers;

public abstract class PageControllerBase<T> : PageController<T> where T : PageBase
{ }
