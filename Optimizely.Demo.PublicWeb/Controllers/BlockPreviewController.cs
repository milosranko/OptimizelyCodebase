using EPiServer.Framework.DataAnnotations;
using EPiServer.Framework.Web;
using EPiServer.Framework.Web.Mvc;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Optimizely.Demo.ContentTypes.Models.Blocks.Base;
using Optimizely.Demo.ContentTypes.Pages;
using Optimizely.Demo.Core.Models.ViewModels;

namespace Optimizely.Demo.PublicWeb.Controllers;

[TemplateDescriptor(
    Inherited = true,
    TemplateTypeCategory = TemplateTypeCategories.MvcController,
    Tags = new[] { RenderingTags.Preview, RenderingTags.Edit },
    AvailableWithoutTag = true)]
[VisitorGroupImpersonation]
[RequireClientResources]
public class BlockPreviewController : ActionControllerBase, IRenderTemplate<BlockBase>
{
    private readonly IContentLoader _contentLoader;
    private readonly TemplateResolver _templateResolver;
    private readonly DisplayOptions _displayOptions;

    public BlockPreviewController(IContentLoader contentLoader, TemplateResolver templateResolver, DisplayOptions displayOptions)
    {
        _contentLoader = contentLoader;
        _templateResolver = templateResolver;
        _displayOptions = displayOptions;
    }

    public IActionResult Index(IContent currentContent)
    {
        //As the layout requires a page for title etc we "borrow" the start page
        var startPage = _contentLoader.Get<StartPage>(SiteDefinition.Current.StartPage);
        var model = new BlockPreviewModel(startPage, currentContent);
        var supportedDisplayOptions = _displayOptions
            .Select(x => new { x.Tag, x.Name, Supported = SupportsTag(currentContent, x.Tag) })
            .ToList();

        if (supportedDisplayOptions.Any(x => x.Supported))
        {
            foreach (var displayOption in supportedDisplayOptions)
            {
                var contentArea = new ContentArea();

                contentArea.Items.Add(new ContentAreaItem
                {
                    ContentLink = currentContent.ContentLink
                });

                //var areaModel = new BlockPreviewModel.PreviewArea
                //{
                //    Supported = displayOption.Supported,
                //    AreaTag = displayOption.Tag,
                //    AreaName = displayOption.Name,
                //    ContentArea = contentArea
                //};

                //model.Areas.Add(areaModel);
            }
        }

        return View(model);
    }

    private bool SupportsTag(IContent content, string tag)
    {
        var templateModel = _templateResolver.Resolve(
            HttpContext,
            content.GetOriginalType(),
            content,
            TemplateTypeCategories.MvcPartial,
            tag);

        return templateModel != null;
    }
}
