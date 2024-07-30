using EPiServer.DataAbstraction;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using Optimizely.Demo.ContentTypes.Models.Media;
using Optimizely.Demo.ContentTypes.Models.Pages.Interfaces;

namespace Optimizely.Demo.Core.Business.Rendering;

[ServiceConfiguration(typeof(IViewTemplateModelRegistrator))]
public class TemplateCoordinator : IViewTemplateModelRegistrator
{
	public const string BlockFolder = "~/Views/Shared/Blocks/";
	public const string PagePartialsFolder = "~/Views/Shared/PagePartials/";

	public void Register(TemplateModelCollection viewTemplateModelRegistrator)
	{
		viewTemplateModelRegistrator.Add(typeof(ImageFile), new TemplateModel
		{
			Name = "ImageFile",
			Tags = new[] { UIHint.Image },
			AvailableWithoutTag = true,
			Path = "~/Views/Shared/DisplayTemplates/ImageFile.cshtml"
		});

		//viewTemplateModelRegistrator.Add(typeof(TeaserBlock), new TemplateModel
		//{
		//    Name = "TeaserBlockWide",
		//    Tags = new[] { Globals.ContentAreaTags.WideWidth, Globals.ContentAreaTags.FullWidth },
		//    AvailableWithoutTag = false,
		//});

		//viewTemplateModelRegistrator.Add(typeof(ArticlePage), new TemplateModel
		//{
		//    //Name = "ArticleCard",
		//    Tags = new[] { "ArticleCard" },
		//    //TemplateTypeCategory = TemplateTypeCategories.MvcPartialComponent,
		//    //AvailableWithoutTag = false,
		//});
	}

	public static void OnTemplateResolved(object? sender, TemplateResolverEventArgs args)
	{
		if (args.ItemToRender is IContainerPage && args.SelectedTemplate != null)
		{
			args.SelectedTemplate = null;
		}
	}

	private static string BlockPath(string fileName) => $"{BlockFolder}{fileName}";

	private static string PagePartialPath(string fileName) => $"{PagePartialsFolder}{fileName}";
}
