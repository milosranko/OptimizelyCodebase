using EPiServer.Core;

namespace Optimizely.Demo.Core.Models.Pages.Interfaces;

public interface IListable : IContent
{
    int PageSize { get; set; }
}
