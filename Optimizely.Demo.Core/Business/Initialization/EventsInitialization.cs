using EPiServer;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;

namespace Optimizely.Demo.PublicWeb.Business.Initialization;

[InitializableModule]
public class EventsInitialization : IInitializableModule
{
    private bool _eventsAttached = false;

    public void Initialize(InitializationEngine context)
    {
        if (!_eventsAttached)
        {
            // Attach event handler to when a page has been created
            ServiceLocator.Current.GetInstance<IContentEvents>().PublishingContent += PublishingContent;
            ServiceLocator.Current.GetInstance<IContentEvents>().PublishedContent += PublishedContent;

            _eventsAttached = true;
        }
    }

    public void Uninitialize(InitializationEngine context)
    {
        if (_eventsAttached)
        {
            // Attach event handler to when a page has been created
            ServiceLocator.Current.GetInstance<IContentEvents>().PublishingContent -= PublishingContent;
            ServiceLocator.Current.GetInstance<IContentEvents>().PublishedContent -= PublishedContent;

            _eventsAttached = false;
        }
    }

    private void PublishingContent(object? sender, ContentEventArgs e)
    {
    }

    private void PublishedContent(object? sender, ContentEventArgs e)
    {
    }
}
