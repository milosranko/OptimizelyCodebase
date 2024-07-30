using Microsoft.AspNetCore.Mvc;
using Optimizely.Demo.ContentTypes.Blocks;
using Optimizely.Demo.Core.Components;
using Optimizely.Demo.Core.Models.ViewModels;
using Optimizely.Demo.PublicWeb.Models.ViewModels;

namespace Optimizely.Demo.PublicWeb.Components;

public class TeaserBlockComponent : BlockComponentBase<TeaserBlock, BlockViewModel<TeaserBlock>>
{
    protected override IViewComponentResult InvokeComponent(TeaserBlock currentContent)
    {
        var model = new ModalViewModel
        {
            Caller = typeof(TeaserBlock),
            Heading = currentContent.Heading,
            LeadText = currentContent.LeadText,
            MainBody = new XhtmlString(),
            Image = currentContent.Image
        };

        return View("~/Views/Shared/Partials/Modal.cshtml", model);
    }
}
