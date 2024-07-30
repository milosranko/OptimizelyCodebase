using EPiServer.Web.Mvc;
using Optimizely.Demo.ContentTypes.Models.Pages.Base;

namespace Optimizely.Demo.Core.Controllers;

public abstract class PageControllerBase<T> : PageController<T> where T : PageBase
{ }
