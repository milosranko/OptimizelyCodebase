using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Optimizely.Demo.ContentTypes.Models.Pages.Base;
using Optimizely.Demo.Core.Business;
using Optimizely.Demo.Core.Models.ViewModels;

namespace Optimizely.Demo.PublicWeb.Filters;

public class PageContextActionFilter : IResultFilter
{
    private readonly PageViewContextFactory _contextFactory;

    public PageContextActionFilter(PageViewContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Controller is Controller controller &&
            controller.ViewData.Model is IPageViewModel<PageBase> model)
        {
            model.Layout = _contextFactory.CreateLayout(model.CurrentPage);
        }
    }

    public void OnResultExecuted(ResultExecutedContext context)
    { }
}
