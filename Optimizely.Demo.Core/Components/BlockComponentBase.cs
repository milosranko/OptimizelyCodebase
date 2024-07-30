using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Optimizely.Demo.ContentTypes.Models.Blocks.Base;
using Optimizely.Demo.Core.Models.ViewModels;

namespace Optimizely.Demo.Core.Components;

public abstract class BlockComponentBase<TBlock, TModel> : BlockComponent<TBlock>
	where TBlock : BlockBase
	where TModel : BlockViewModel<TBlock>, new()
{
	public PageData CurrentPage => ServiceLocator.Current.GetInstance<IPageRouteHelper>().Page;
}
