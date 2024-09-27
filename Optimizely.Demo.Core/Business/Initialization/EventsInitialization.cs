using EPiServer;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Optimizely.Demo.Core.Models.Pages;
using Optimizely.Demo.Core.Services.SiteSettings;

namespace Optimizely.Demo.Core.Business.Initialization;

[InitializableModule]
public class EventsInitialization : IInitializableModule
{
    private bool _eventsAttached = false;

    public void Initialize(InitializationEngine context)
    {
        if (!_eventsAttached)
        {
            // Attach event handler to when a page has been created
            ServiceLocator.Current.GetInstance<IContentEvents>().PublishedContent += PublishedContent;

            _eventsAttached = true;
        }
    }

    public void Uninitialize(InitializationEngine context)
    {
        if (_eventsAttached)
        {
            // Attach event handler to when a page has been created
            ServiceLocator.Current.GetInstance<IContentEvents>().PublishedContent -= PublishedContent;

            _eventsAttached = false;
        }
    }

    private void PublishedContent(object? sender, ContentEventArgs e)
    {
        #region SiteSettingsPageBase events

        if (e.Content is SiteSettingsPageBase siteSettings)
        {
            ServiceLocator.Current.GetInstance<ISiteSettingsManager>().RemoveFromCache(siteSettings.ParentLink.ID);
        }

        #endregion
    }
}
