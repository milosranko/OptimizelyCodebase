using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.ClientConventions;
using EPiServer.Find.Cms;
using EPiServer.Find.Cms.Conventions;
using EPiServer.Find.Cms.Json;
using EPiServer.Find.Cms.Module;
using EPiServer.Find.Framework;
using EPiServer.Find.UnifiedSearch;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using Optimizely.Demo.Core.Models.Pages.Base;

namespace Optimizely.Demo.Core.Business.Initialization;

[InitializableModule]
[ModuleDependency(typeof(IndexingModule))]
public class SearchClientInitialization : IInitializableModule
{
    public void Initialize(InitializationEngine context)
    {
        SearchClient.Instance.Conventions.UnifiedSearchRegistry
            .ForInstanceOf<PageBasePublic>()
            .AlwaysApplyFilter(x => x.BuildFilter<PageBasePublic>().And(y => y.IsDeleted.Match(false)).And(y => y.ExcludeFromSiteSearchResults.Match(false)));
        //.ProjectMetaDataFrom(x => new Dictionary<string, IndexValue> { { nameof(x.SearchCategories), string.Join(",", x.SearchCategories) } });
        SearchClient.Instance.Conventions.ForInstancesOf<ContentArea>().ModifyContract(x => x.Converter = new MaxDepthContentAreaConverter(1));

        ContentIndexer.Instance.Conventions.ForInstancesOf<PageBasePublic>().ShouldIndex(x => !x.ExcludeFromSiteSearchResults);
        ContentIndexer.Instance.Conventions.ShouldIndexInContentAreaConvention = new DefaultShouldIndexInContentAreaConvention();
        ContentIndexer.Instance.Conventions.ForInstancesOf<BlockData>().ShouldIndex(x => true);
        ContentIndexer.Instance.Conventions.ForInstancesOf<IContentMedia>().ShouldIndex(x => false);
        ContentIndexer.Instance.Conventions.ForInstancesOf<ContainerBase>().ShouldIndex(x => false);
    }

    public void Uninitialize(InitializationEngine context)
    {
    }
}
