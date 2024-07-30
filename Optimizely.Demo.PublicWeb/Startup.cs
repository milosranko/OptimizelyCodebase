using CmsContentScaffolding.Optimizely.Extensions;
using CmsContentScaffolding.Optimizely.Helpers;
using CmsContentScaffolding.Optimizely.Models;
using CmsContentScaffolding.Optimizely.Startup;
using CmsContentScaffolding.Shared.Resources;
using EPiServer.Cms.Shell;
using EPiServer.Cms.Shell.UI;
using EPiServer.Cms.Shell.UI.Configurations;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Framework.Web.Resources;
using EPiServer.Scheduler;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Routing;
using Geta.NotFoundHandler.Infrastructure.Configuration;
using Geta.NotFoundHandler.Infrastructure.Initialization;
using Geta.NotFoundHandler.Optimizely.Infrastructure.Configuration;
using Geta.NotFoundHandler.Optimizely.Infrastructure.Initialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Optimizely.Demo.ContentTypes.Blocks;
using Optimizely.Demo.ContentTypes.Constants;
using Optimizely.Demo.ContentTypes.Models.Media;
using Optimizely.Demo.ContentTypes.Pages;
using Optimizely.Demo.Core.Business.Initialization;
using Optimizely.Demo.Core.Business.Rendering;
using Optimizely.Demo.PublicWeb.Api;
using Optimizely.Demo.PublicWeb.Api.Services;
using Optimizely.Demo.PublicWeb.Extensions;
using Optimizely.Demo.PublicWeb.Filters;
using System.Globalization;
using System.Net;

namespace Optimizely.Demo.PublicWeb;

public class Startup
{
    private readonly IWebHostEnvironment _webHostingEnvironment;
    private readonly IConfiguration _configuration;

    public Startup(IWebHostEnvironment webHostingEnvironment, IConfiguration configuration)
    {
        _webHostingEnvironment = webHostingEnvironment;
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        Console.WriteLine("Registering services...");

        if (_webHostingEnvironment.IsDevelopment())
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(_webHostingEnvironment.ContentRootPath, "App_Data"));

            services
                .Configure<SchedulerOptions>(options => options.Enabled = false)
                .Configure<ClientResourceOptions>(uiOptions => uiOptions.Debug = true);
        }

        //If added before Optimizely it will remain the only registration
        //Use TryAdd if you want to ensure just one registration
        //services.AddSingleton<ContentAreaRenderer, CustomContentAreaRenderer>();
        //services.AddTransient<IContentQuery, CustomGetChildrenQuery>();
        services
            .AddCmsAspNetIdentity<ApplicationUser>()
            .AddCms()
            .AddAdminUserRegistration(options => options.Behavior = RegisterAdminUserBehaviors.LocalRequestsOnly)
            .AddEmbeddedLocalization<Startup>()
            .AddCustomTinyMceConfiguration()
            .AddCmsContentScaffolding()
            .AddNotFoundHandler(o =>
            {
                o.UseSqlServer(_configuration.GetConnectionString("EPiServerDB"));
                o.Logging = LoggerMode.On;
            })
            .AddOptimizelyNotFoundHandler(o => o.AutomaticRedirectsEnabled = true)
            .AddCors();

        services.AddTransient<IApiService, ApiService>();

        #region Optimizely DXP

        //if (!_webHostingEnvironment.IsDevelopment())
        //{
        //	services.AddAzureBlobProvider(o =>
        //	{
        //		o.ConnectionString = _configuration.GetSection("ConnectionStrings").GetValue<string>("EPiServerAzureBlobs");
        //		o.ContainerName = $"container-name";
        //	});

        //	services.AddCmsCloudPlatformSupport(_configuration);
        //}

        #endregion

        services
            .Configure<DisplayOptions>(displayOption =>
            {
                displayOption.Add("full", "/displayoptions/full", Globals.ContentAreaTags.FullWidth, string.Empty, "epi-icon__layout--full");
                displayOption.Add("wide", "/displayoptions/wide", Globals.ContentAreaTags.WideWidth, string.Empty, "epi-icon__layout--wide");
                displayOption.Add("half", "/displayoptions/half", Globals.ContentAreaTags.HalfWidth, string.Empty, "epi-icon__layout--half");
                displayOption.Add("narrow", "/displayoptions/narrow", Globals.ContentAreaTags.NarrowWidth, string.Empty, "epi-icon__layout--narrow");
            })
            .Configure<RazorViewEngineOptions>(options => options.ViewLocationExpanders.Add(new SiteViewEngineLocationExpander()))
            .Configure<MvcOptions>(options => options.Filters.Add<PageContextActionFilter>())
            .Configure<UploadOptions>(options => options.FileSizeLimit = 62914560);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        Console.WriteLine("Configuring services...");

        #region CMS Content Scaffolding

        Console.WriteLine("Content Scaffolding...");

        app.UseCmsContentScaffolding(
            builderOptions: o =>
            {
                o.SiteName = "SITE 1 SR";
                o.Language = CultureInfo.GetCultureInfo("sr");
                o.SiteHost = "https://localhost:5000";
                o.StartPageType = typeof(StartPage);
                o.BuildMode = BuildMode.OnlyIfEmpty;
                o.PublishContent = true;
                o.Roles = new Dictionary<string, AccessLevel>
                {
                    { "Site1Editors", AccessLevel.Read | AccessLevel.Create | AccessLevel.Edit }
                };
                o.Users =
                [
                    new("Site1User", "Site1User@test.com", "Test@1234", ["Site1Editors"])
                ];
            },
            builder: b =>
            {
                b.UseAssets(ContentReference.SiteBlockFolder)
                .WithFolder("Folder 1", l1 =>
                l1
                .WithFolder("Folder 1_1", l2 => l2.WithBlock<TeaserBlock>("Teaser 1", x => x.Heading = "Teaser Heading"))
                .WithMedia<VideoFile>(x => x.Name = "Test video", ResourceHelpers.GetVideoStream(), ".mp4"));

                b.UsePages()
                .WithStartPage<StartPage>(p =>
                {
                    p.Name = "Site 1 Start Page";
                    p.MainContentArea
                    .AddItems<TeaserBlock>("Teaser", b =>
                    {
                        b.Heading = ResourceHelpers.Faker.Lorem.Slug();
                        b.LeadText = ResourceHelpers.Faker.Lorem.Paragraph();
                        b.Image = PropertyHelpers.GetOrAddMedia<ImageFile>("Image 1", ".png", ResourceHelpers.GetImageStream());
                    }, 3)
                    .AddItem<TeaserBlock>(b =>
                    {
                        b.Heading = ResourceHelpers.Faker.Lorem.Slug();
                        b.LeadText = ResourceHelpers.Faker.Lorem.Paragraph();
                        b.Image = PropertyHelpers.GetOrAddMedia<ImageFile>("Image 1", ".png", ResourceHelpers.GetImageStream());
                    });
                }, l1 =>
                {
                    l1
                    .WithPage<ArticlePage>(p =>
                    {
                        p.Name = "Article1_1";
                        p.Heading = ResourceHelpers.Faker.Lorem.Slug();
                        p.LeadText = ResourceHelpers.Faker.Lorem.Paragraph();
                        p.MainContent
                        .AddStringFragment(ResourceHelpers.Faker.Lorem.Paragraphs(2))
                        .AddContentFragment(PropertyHelpers.GetOrAddMedia<ImageFile>("Image 1", ".png", ResourceHelpers.GetImageStream()))
                        .AddStringFragment(ResourceHelpers.Faker.Lorem.Paragraphs(3));
                        p.TopImage = PropertyHelpers.GetOrAddMedia<ImageFile>("Image 1", ".png", ResourceHelpers.GetImageStream());
                        p.MainContentArea
                        .AddItem<TeaserBlock>(b =>
                        {
                            b.Heading = ResourceHelpers.Faker.Lorem.Slug();
                            b.LeadText = ResourceHelpers.Faker.Lorem.Paragraph();
                            b.Image = PropertyHelpers.GetOrAddMedia<ImageFile>("Image 1", ".png", ResourceHelpers.GetImageStream());
                        })
                        .AddItem<AccordionContainerBlock>("Accordion Container", b =>
                        {
                            b.Heading = ResourceHelpers.Faker.Lorem.Slug();
                            b.Items.AddItems<AccordionItemBlock>("Accordion Item", b1 =>
                            {
                                b1.Heading = ResourceHelpers.Faker.Lorem.Slug();
                                b1.Image = PropertyHelpers.GetOrAddMedia<ImageFile>("Image 1", ".png", ResourceHelpers.GetImageStream());
                                b1.LeadText = ResourceHelpers.Faker.Lorem.Paragraph();
                            }, 5);
                        })
                        .AddItem<ImageFile>(options: i =>
                        {
                            i.Name = "Test Image";
                            i.ContentLink = PropertyHelpers.GetOrAddMedia<ImageFile>("Image 1", ".png", ResourceHelpers.GetImageStream());
                        });
                    }, l2 =>
                    {
                        l2
                        .WithPage<ArticlePage>(p =>
                        {
                            p.Name = "Article2_1";
                        })
                        .WithPage<ArticlePage>(options: l3 => { l3.WithPages<ArticlePage>(totalPages: 10); });
                    })
                    .WithPage<EventsCollectionPage>(options: l2 =>
                    {
                        l2
                        .WithPage<YearContainerPage>(p =>
                        {
                            p.Name = "2022";
                        }, options: l3 =>
                        {
                            l3
                            .WithPage<MonthContainerPage>(p =>
                            {
                                p.Name = "01";
                            }, l4 => { l4.WithPages<EventPage>(totalPages: 10); })
                            .WithPage<MonthContainerPage>(p =>
                            {
                                p.Name = "02";
                            }, l4 => { l4.WithPages<EventPage>(totalPages: 10); })
                            .WithPage<MonthContainerPage>(p =>
                            {
                                p.Name = "03";
                            }, l4 => { l4.WithPages<EventPage>(totalPages: 10); })
                            .WithPage<MonthContainerPage>(p =>
                            {
                                p.Name = "04";
                            }, l4 => { l4.WithPages<EventPage>(totalPages: 10); })
                            .WithPage<MonthContainerPage>(p =>
                            {
                                p.Name = "05";
                            }, l4 => { l4.WithPages<EventPage>(totalPages: 10); })
                            .WithPage<MonthContainerPage>(p =>
                            {
                                p.Name = "06";
                            }, l4 => { l4.WithPages<EventPage>(totalPages: 10); })
                            .WithPage<MonthContainerPage>(p =>
                            {
                                p.Name = "07";
                            }, l4 => { l4.WithPages<EventPage>(totalPages: 10); })
                            .WithPage<MonthContainerPage>(p =>
                            {
                                p.Name = "08";
                            }, l4 => { l4.WithPages<EventPage>(totalPages: 10); })
                            .WithPage<MonthContainerPage>(p =>
                            {
                                p.Name = "09";
                            }, l4 => { l4.WithPages<EventPage>(totalPages: 10); })
                            .WithPage<MonthContainerPage>(p =>
                            {
                                p.Name = "10";
                            }, l4 => { l4.WithPages<EventPage>(totalPages: 10); })
                            .WithPage<MonthContainerPage>(p =>
                            {
                                p.Name = "11";
                            }, l4 => { l4.WithPages<EventPage>(totalPages: 10); })
                            .WithPage<MonthContainerPage>(p =>
                            {
                                p.Name = "12";
                            }, l4 => { l4.WithPages<EventPage>(totalPages: 10); });
                        });
                    });
                })
                .WithPage<NotFoundPage>(p =>
                {
                    p.Name = "404";
                    p.Heading = "Page is NOT found!";
                });
            });

        app.UseCmsContentScaffolding(
            builderOptions: o =>
            {
                o.SiteName = "SITE 2 EN";
                o.Language = CultureInfo.GetCultureInfo("en");
                o.SiteHost = "https://localhost:5001";
                o.StartPageType = typeof(StartPage);
                o.BuildMode = BuildMode.OnlyIfEmpty;
                o.PublishContent = true;
                o.Roles = new Dictionary<string, AccessLevel>
                {
                    { "Site2Editors", AccessLevel.Read | AccessLevel.Create | AccessLevel.Edit | AccessLevel.Publish }
                };
                o.Users =
                [
                    new("Site2User", "Site2User@test.com", "Test@1234", ["Site2Editors"])
                ];
            },
            builder: b =>
            {
                b.UsePages()
                .WithStartPage<StartPage>(p =>
                {
                    p.Name = "Site 2 Start Page";
                    p.Heading = "Some Heading";
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
                    .WithPage<ArticlePage>(p =>
                    {
                        p.Name = "Article1_1";
                        p.Heading = ResourceHelpers.Faker.Lorem.Slug();
                        p.LeadText = ResourceHelpers.Faker.Lorem.Paragraph();
                        p.MainContent.AddStringFragment(ResourceHelpers.GetHtmlText());
                        p.TopImage = PropertyHelpers.GetOrAddMedia<ImageFile>("Image 1", ".png", ResourceHelpers.GetImageStream());
                        p.MainContentArea
                        .AddItem<TeaserBlock>(b =>
                        {
                            b.Heading = ResourceHelpers.Faker.Lorem.Slug();
                            b.LeadText = ResourceHelpers.Faker.Lorem.Paragraph();
                            b.Image = PropertyHelpers.GetOrAddMedia<ImageFile>("Image 1", ".png", ResourceHelpers.GetImageStream());
                        })
                        .AddItem<AccordionContainerBlock>("Accordion Container", b =>
                        {
                            b.Heading = ResourceHelpers.Faker.Lorem.Slug();
                            b.Items.AddItems<AccordionItemBlock>("Accordion Item", b1 =>
                            {
                                b1.Heading = ResourceHelpers.Faker.Lorem.Slug();
                                b1.Image = PropertyHelpers.GetOrAddMedia<ImageFile>("Image 1", ".png", ResourceHelpers.GetImageStream());
                                b1.LeadText = ResourceHelpers.Faker.Lorem.Paragraph();
                            }, 5);
                        })
                        .AddItem<ImageFile>(options: i =>
                        {
                            i.Name = "Test Image";
                            i.ContentLink = PropertyHelpers.GetOrAddMedia<ImageFile>("Image 1", ".png", ResourceHelpers.GetImageStream());
                        });
                    }, l2 =>
                    {
                        l2
                        .WithPage<ArticlePage>(p =>
                        {
                            p.Name = "Article2_1";
                        })
                        .WithPage<ArticlePage>(options: l3 => { l3.WithPages<ArticlePage>(totalPages: 20); });
                    })
                    .WithPage<EventsCollectionPage>(options: l2 =>
                    {
                        l2
                        .WithPage<YearContainerPage>(p =>
                        {
                            p.Name = "2022";
                        }, options: l3 =>
                        {
                            l3
                            .WithPage<MonthContainerPage>(p =>
                            {
                                p.Name = "January";
                            }, l4 => { l4.WithPages<EventPage>(totalPages: 10); })
                            .WithPage<MonthContainerPage>(p =>
                            {
                                p.Name = "February";
                            }, l4 => { l4.WithPages<EventPage>(totalPages: 10); })
                            .WithPage<MonthContainerPage>(p =>
                            {
                                p.Name = "March";
                            }, l4 => { l4.WithPages<EventPage>(totalPages: 10); })
                            .WithPage<MonthContainerPage>(p =>
                            {
                                p.Name = "April";
                            }, l4 => { l4.WithPages<EventPage>(totalPages: 10); })
                            .WithPage<MonthContainerPage>(p =>
                            {
                                p.Name = "May";
                            }, l4 => { l4.WithPages<EventPage>(totalPages: 10); })
                            .WithPage<MonthContainerPage>(p =>
                            {
                                p.Name = "June";
                            }, l4 => { l4.WithPages<EventPage>(totalPages: 10); })
                            .WithPage<MonthContainerPage>(p =>
                            {
                                p.Name = "July";
                            }, l4 => { l4.WithPages<EventPage>(totalPages: 10); })
                            .WithPage<MonthContainerPage>(p =>
                            {
                                p.Name = "August";
                            }, l4 => { l4.WithPages<EventPage>(totalPages: 10); })
                            .WithPage<MonthContainerPage>(p =>
                            {
                                p.Name = "September";
                            }, l4 => { l4.WithPages<EventPage>(totalPages: 10); })
                            .WithPage<MonthContainerPage>(p =>
                            {
                                p.Name = "October";
                            }, l4 => { l4.WithPages<EventPage>(totalPages: 10); })
                            .WithPage<MonthContainerPage>(p =>
                            {
                                p.Name = "November";
                            }, l4 => { l4.WithPages<EventPage>(totalPages: 10); })
                            .WithPage<MonthContainerPage>(p =>
                            {
                                p.Name = "December";
                            }, l4 => { l4.WithPages<EventPage>(totalPages: 10); });
                        });
                    });
                });
            });

        #endregion

        #region HTTP 500 and 404 handlers

        //HTTP 500 handler
        app.UseGlobalExceptionHandler(env.IsDevelopment(), "/500.html");

        //HTTP 404 handler
        app.Use(async (context, next) =>
        {
            await next();

            if (context.Response.StatusCode == (int)HttpStatusCode.NotFound && !context.Request.Path.StartsWithSegments("/api"))
            {
                context.Request.Path = "/404";
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await next();
            }
        });

        #region Geta NotFound handler

        app.UseNotFoundHandler();
        app.UseOptimizelyNotFoundHandler();

        #endregion

        #endregion

        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapContent();
            endpoints.MapDefaultControllerRoute();
            //Map minimal API endpoints
            endpoints.AddApiEndpoints();
            //Geta NotFound Admin UI
            endpoints.MapRazorPages();
        });

        Console.WriteLine("Application started...");
    }
}