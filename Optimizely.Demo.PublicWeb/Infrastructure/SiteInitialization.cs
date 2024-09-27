using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Mvc.Html;
using Optimizely.Demo.Core.Business.Rendering;

namespace Optimizely.Demo.PublicWeb.Infrastructure;

[ModuleDependency(typeof(InitializationModule))]
public class SiteInitialization : IConfigurableModule
{
    public void ConfigureContainer(ServiceConfigurationContext context)
    {
        //Register services here
        context.Services.AddSingleton<ContentAreaRenderer, CustomContentAreaRenderer>();
        //var noContentAreaRenderer = context.Services.Where(x => x.ServiceType == typeof(ContentAreaRenderer)).ToArray();
        //var contentQuery = context.Services.Where(x => x.ServiceType == typeof(IContentQuery)).ToArray();
        //context.Services.AddTransient<IContentQuery, CustomGetChildrenQuery>();

        //Post services registration
        //context.ConfigurationComplete += (o, e) =>
        //{
        //    //context.Services.AddSingleton<ContentAreaRenderer, CustomContentAreaRenderer>();
        //    context.Services
        //    //.AddSingleton<ContentAreaRenderer, CustomContentAreaRenderer>()
        //    //.Intercept<ContentAreaRenderer>((locator, defaultContentAreaRenderer) => services.GetRequiredService<CustomContentAreaRenderer>())
        //    .AddTransient<IContentRenderer, ErrorHandlingContentRenderer>();

        //    var customContentAreaRenderer = context.Services.Where(x => x.ServiceType == typeof(ContentAreaRenderer)).ToArray();

        //    //context.Services.AddSingleton<MenuAssembler, CustomMenuAssembler>();
        //    //Replace service registration will replace all implementations
        //    //Intercept will replace all implementations of certain type with new one at the runtime when requested
        //    //context.Services.Intercept<IContentQuery>((services, query) => services.GetRequiredService<CustomGetChildrenQuery>());
        //    //context.Services.Replace(new Microsoft.Extensions.DependencyInjection.ServiceDescriptor(typeof(IContentQuery), typeof(CustomGetChildrenQuery), ServiceLifetime.Transient));
        //    context.Services.Remove(context.Services.Single(x => x.ServiceType == typeof(IContentQuery) && x.ImplementationType == typeof(GetChildrenQuery)));
        //    context.Services.Add(new Microsoft.Extensions.DependencyInjection.ServiceDescriptor(typeof(IContentQuery), typeof(CustomGetChildrenQuery), ServiceLifetime.Transient));
        //};
    }

    public void Initialize(InitializationEngine context) =>
        context.Locate.Advanced.GetInstance<ITemplateResolverEvents>().TemplateResolved += TemplateCoordinator.OnTemplateResolved;

    public void Uninitialize(InitializationEngine context) =>
        context.Locate.Advanced.GetInstance<ITemplateResolverEvents>().TemplateResolved -= TemplateCoordinator.OnTemplateResolved;
}
