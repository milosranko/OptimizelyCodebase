using CmsContentScaffolding.Optimizely.Extensions;
using CmsContentScaffolding.Optimizely.Helpers;
using CmsContentScaffolding.Optimizely.Models;
using CmsContentScaffolding.Optimizely.Startup;
using CmsContentScaffolding.Shared.Resources;
using EPiServer;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Filters;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Optimizely.Demo.ContentTypes.Blocks;
using Optimizely.Demo.ContentTypes.Models.Media;
using Optimizely.Demo.ContentTypes.Pages;
using Optimizely.Demo.Core.Business.Caching;
using Optimizely.Demo.Core.Models.ViewModels;
using Optimizely.Demo.Core.Services.SiteSettings;
using Optimizely.Demo.PublicWeb.Controllers;
using System.Globalization;

namespace Optimizely.Demo.Tests;

[TestClass]
public class OptimizelyTests
{
    private const string Language = "sr";

    [ClassInitialize]
    public static void Initialize(TestContext context)
    {
        var builder = Host
            .CreateDefaultBuilder()
            .ConfigureCmsDefaults()
            .ConfigureAppConfiguration((context, config) =>
            {
                config
                .AddConfiguration(context.Configuration)
                .AddEnvironmentVariables()
                .AddJsonFile($"appsettings.unittests.json", false, true)
                .Build();
            })
            .ConfigureServices((context, services) =>
            {
                services
                .AddSingleton<IHttpContextFactory, DefaultHttpContextFactory>()
                .AddCmsAspNetIdentity<ApplicationUser>()
                .AddCms()
                .AddFind()
                .AddCmsContentScaffolding()
                .AddTransient<ICacheService, CacheService>()
                .AddTransient<ISiteSettingsManager, SiteSettingsManager>();

                Globals.Services = services.BuildServiceProvider();

                var dbContext = Globals.Services.GetRequiredService<ApplicationDbContext<ApplicationUser>>();
                dbContext.Database.EnsureCreated();
            })
            .ConfigureWebHost(webHost =>
            {
                webHost.UseTestServer();
                webHost.Configure(app =>
                {
                    app.UseCmsContentScaffolding(
                        builderOptions: o =>
                        {
                            o.StartPageType = typeof(StartPage);
                            o.SiteHost = "http://localhost:5001";
                            o.Language = CultureInfo.GetCultureInfo(Language);
                            o.BuildMode = BuildMode.OnlyIfEmpty;
                            o.PublishContent = true;
                        },
                        builder: b =>
                        {
                            b.UsePages()
                            .WithStartPage<StartPage>(p =>
                            {
                                p.Name = "Home Page";
                                p.MainContentArea
                                .AddItem<TeaserBlock>()
                                .AddItem<TeaserBlock>(b =>
                                {
                                    b.Heading = ResourceHelpers.Faker.Lorem.Slug();
                                    b.LeadText = ResourceHelpers.Faker.Lorem.Paragraph();
                                    b.Image = PropertyHelpers.GetOrAddMedia<ImageFile>("Image 1", ".png", ResourceHelpers.GetImageStream());
                                });
                            }, l1 =>
                            {
                                l1
                                .WithPage<SiteSettingsPage>(p =>
                                {
                                    p.Name = "Site settings";
                                    p.SiteName = "DEMO SITE";
                                    p.FooterScripts = [new() { Name = "Test script", Value = "<script>console.log('script test')</script>" }];
                                })
                                .WithPage<ArticlePage>(p =>
                                {
                                    p.Name = "Article1_1";
                                    p.MainContent.AddStringFragment(ResourceHelpers.GetHtmlText());
                                }, l2 =>
                                {
                                    l2
                                    .WithPage<ArticlePage>(page =>
                                    {
                                        page.Name = "Article2_1";
                                    })
                                    .WithPage<ArticlePage>(options: l3 =>
                                    {
                                        l3.WithPages<ArticlePage>(totalPages: 20);
                                    });
                                })
                                .WithPages<ArticlePage>(totalPages: 50);
                            })
                            .WithPage<ArticlePage>()
                            .WithPages<ArticlePage>(page =>
                            {
                                page.Name = "Article2";
                            }, 50);
                        });
                });
            });

        builder.Build().Start();
    }

    [ClassCleanup]
    public static void Uninitialize()
    {
        var dbContext = Globals.Services.GetRequiredService<ApplicationDbContext<ApplicationUser>>();
        dbContext.Database.EnsureDeleted();
    }

    [TestMethod]
    public void InitializationTest_ShouldGetStartPage()
    {
        //Arrange
        var contentLoader = ServiceLocator.Current.GetRequiredService<IContentLoader>();
        var siteDefinitionRepository = ServiceLocator.Current.GetRequiredService<ISiteDefinitionRepository>();

        //Act
        var pages = contentLoader.GetDescendents(ContentReference.RootPage);
        var siteDefinition = siteDefinitionRepository
            .List()
            .Where(x => x.GetHosts(CultureInfo.GetCultureInfo(Language), false).Any())
            .Single();
        var startPage = contentLoader.Get<StartPage>(siteDefinition.StartPage);

        //Assert
        Assert.IsNotNull(pages);
        Assert.IsTrue(pages.Any());
        Assert.IsNotNull(startPage?.MainContentArea);
        Assert.IsFalse(startPage.MainContentArea.IsEmpty);
    }

    [TestMethod]
    public void PerformanceTest_ShouldGetAllArticlePagesUsingPageCriteriaQueryService()
    {
        //Arrange
        var contentTypeRepository = ServiceLocator.Current.GetInstance<IContentTypeRepository>();
        var pageCriteriaQueryService = ServiceLocator.Current.GetInstance<IPageCriteriaQueryService>();
        var criterias = new PropertyCriteriaCollection
        {
            new PropertyCriteria
            {
                Name = "PageTypeID",
                Type = PropertyDataType.PageType,
                Condition = CompareCondition.Equal,
                Value = contentTypeRepository.Load<ArticlePage>().ID.ToString(),
                Required = true
            }
        };

        //Act
        var res = pageCriteriaQueryService.FindAllPagesWithCriteria(ContentReference.RootPage, criterias, Language, LanguageSelector.MasterLanguage());

        //Assert
        Assert.IsNotNull(res);
        Assert.IsTrue(res.Count > 50);
    }

    [TestMethod]
    public void PerformanceTest_ShouldGetAllArticlePagesUsingContentLoader()
    {
        //Arrange
        var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();

        //Act
        var res = contentLoader.GetDescendents(ContentReference.RootPage)
            .Where(x =>
            {
                if (contentLoader.TryGet<PageData>(x, out var page))
                {
                    return page is ArticlePage;
                }

                return false;
            })
            .ToArray();

        //Assert
        Assert.IsNotNull(res);
        Assert.IsTrue(res.Length > 50);
    }

    [TestMethod]
    public void StartPageController_ShouldReturnResponse()
    {
        //Arrange
        var contentLoader = ServiceLocator.Current.GetRequiredService<IContentLoader>();
        var siteDefinitionRepository = ServiceLocator.Current.GetRequiredService<ISiteDefinitionRepository>();
        var siteDefinition = siteDefinitionRepository
            .List()
            .Where(x => x.GetHosts(CultureInfo.GetCultureInfo(Language), false).Any())
            .Single();
        var startPage = contentLoader.Get<StartPage>(siteDefinition.StartPage);
        var _controller = new StartPageController();

        //Act
        var res = (ViewResult)_controller.Index(startPage);

        //Assert
        Assert.IsNotNull(res);
        Assert.IsInstanceOfType(res.Model, typeof(PageViewModel<StartPage>));
        Assert.IsNotNull(((PageViewModel<StartPage>)res.Model).CurrentPage.MainContentArea);
    }
}