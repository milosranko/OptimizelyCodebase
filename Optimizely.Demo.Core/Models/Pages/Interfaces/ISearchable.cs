using EPiServer.Core;

namespace Optimizely.Demo.Core.Models.Pages.Interfaces;

public interface ISearchable : IContent
{
    bool ExcludeFromSiteSearchResults { get; set; }
}
