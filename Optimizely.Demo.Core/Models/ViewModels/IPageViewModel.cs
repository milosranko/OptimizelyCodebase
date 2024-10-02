using Optimizely.Demo.Core.Models.Pages.Base;

namespace Optimizely.Demo.Core.Models.ViewModels;

public interface IPageViewModel<out T> where T : PageBase
{
    T CurrentPage { get; }
    LayoutModel Layout { get; set; }
}

public interface IPageViewModel<out T, TViewModel>
    where T : PageBase
    where TViewModel : class
{
    T CurrentPage { get; }
    TViewModel CurrentPageViewModel { get; }
    LayoutModel Layout { get; set; }
}
