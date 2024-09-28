using EPiServer.Core;

namespace Optimizely.Demo.Core.Models.Pages.Interfaces;

public interface ISearchable : IContentData
{
    bool ExcludeFromSiteSearchResults { get; set; }
}
