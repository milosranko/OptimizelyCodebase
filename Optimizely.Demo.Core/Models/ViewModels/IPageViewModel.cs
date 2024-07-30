using Optimizely.Demo.ContentTypes.Models.Pages.Base;

namespace Optimizely.Demo.Core.Models.ViewModels;

public interface IPageViewModel<out T> where T : PageBase
{
	T CurrentPage { get; }
	LayoutModel Layout { get; set; }
}
