using Optimizely.Demo.ContentTypes.Models.Pages.Base;
using System.Text.Json.Serialization;

namespace Optimizely.Demo.Core.Models.ViewModels;

public record PageViewModel<T> : IPageViewModel<T> where T : PageBase
{
	[JsonIgnore]
	public T CurrentPage { get; private set; }
	public LayoutModel Layout { get; set; }

	public PageViewModel(T currentPage)
	{
		CurrentPage = currentPage;
		Layout = new LayoutModel();
	}
}

public static class PageViewModel
{
	/// <summary>
	/// Returns a PageViewModel of type <typeparam name="T"/>.
	/// </summary>
	/// <remarks>
	/// Convenience method for creating PageViewModels without having to specify the type as methods can use type inference while constructors cannot.
	/// </remarks>
	public static PageViewModel<T> Create<T>(T page) where T : PageBase => new(page);
}
