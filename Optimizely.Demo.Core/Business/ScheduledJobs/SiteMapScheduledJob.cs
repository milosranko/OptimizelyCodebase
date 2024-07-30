using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.PlugIn;
using EPiServer.Scheduler;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Optimizely.Demo.ContentTypes.Models.Pages.Base;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Optimizely.Demo.Core.Business.ScheduledJobs;

[ScheduledPlugIn(
    DisplayName = "SiteMap Scheduled Job",
    Description = "Generates sitemap.xml from the current site structure",
    GUID = "78D36CAA-32CF-4AAA-89F1-A87A42CC208C",
    InitialTime = "00:00:00",
    IntervalType = ScheduledIntervalType.Days,
    IntervalLength = 1,
    DefaultEnabled = true,
    Restartable = true)]
public class SiteMapScheduledJob : ScheduledJobBase
{
    private readonly XNamespace SitemapXmlNamespace = "http://www.sitemaps.org/schemas/sitemap/0.9";
    private readonly XNamespace XhtmlNamespace = "http://www.w3.org/1999/xhtml";
    private readonly XNamespace XsiNamespace = "http://www.w3.org/2001/XMLSchema-instance";
    private readonly XNamespace LocationNamespace = "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd";
    private readonly ILogger<SiteMapScheduledJob> _log;
    private readonly IUrlResolver _urlResolver;
    private readonly IContentRepository _contentRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private bool _stopSignaled;

    public SiteMapScheduledJob(
        IUrlResolver urlResolver,
        IContentRepository contentRepository,
        IWebHostEnvironment webHostEnvironment,
        ILogger<SiteMapScheduledJob> log)
    {
        _urlResolver = urlResolver;
        _contentRepository = contentRepository;
        _webHostEnvironment = webHostEnvironment;
        IsStoppable = true;
        _log = log;
    }

    /// <summary>
    /// Called when a user clicks on Stop for a manually started job, or when ASP.NET shuts down.
    /// </summary>
    public override void Stop()
    {
        _stopSignaled = true;
    }

    /// <summary>
    /// Called when a scheduled job executes
    /// </summary>
    /// <returns>A status message to be stored in the database log and visible from admin mode</returns>
    public override string Execute()
    {
        //Call OnStatusChanged to periodically notify progress of job for manually started jobs
        OnStatusChanged($"Starting execution of {GetType()}");

        try
        {
            var doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            var indexElement = new XElement(SitemapXmlNamespace + "urlset",
                                new XAttribute("xmlns", SitemapXmlNamespace),
                                new XAttribute(XNamespace.Xmlns + "xhtml", XhtmlNamespace),
                                new XAttribute(XNamespace.Xmlns + "xsi", XsiNamespace),
                                new XAttribute(XsiNamespace + "schemaLocation", LocationNamespace));

            // Add start page
            var sitemapStartPageElement = new XElement(
                               SitemapXmlNamespace + "url",
                               new XElement(SitemapXmlNamespace + "loc", GetExternalUrl(ContentReference.StartPage)));
            indexElement.Add(sitemapStartPageElement);

            var contentRefs = _contentRepository.GetDescendents(ContentReference.StartPage).ToArray();
            foreach (var content in contentRefs)
            {
                //For long running jobs periodically check if stop is signaled and if so stop execution
                if (_stopSignaled)
                {
                    return "Stop of job was called";
                }

                var sitemapElement = CreateUrlNode(content);
                if (sitemapElement != null)
                {
                    indexElement.Add(sitemapElement);
                }
            }

            doc.Add(indexElement);

            // Write new Sitemap
            byte[] sitemapIndexData;

            using (var ms = new MemoryStream())
            {
                var xtw = new XmlTextWriter(ms, Encoding.UTF8);
                doc.Save(xtw);
                xtw.Flush();
                sitemapIndexData = ms.ToArray();
            }

            var rootFolder = _webHostEnvironment.WebRootPath;
            File.WriteAllBytes(rootFolder + "\\sitemap.xml", sitemapIndexData);

            return "Sitemap successfully created.";
        }
        catch (Exception ex)
        {
            _log.LogError("Could not create sitemap. Exception: ", ex);
            throw;
        }
    }

    private XElement CreateUrlNode(ContentReference page)
    {
        if (page == null || ContentReference.IsNullOrEmpty(page)) return null;
        if (!_contentRepository.TryGet(page, out PageBaseSeo pageBaseSeo)) return null;
        if (pageBaseSeo.ParentLink == ContentReference.WasteBasket) return null;
        if (pageBaseSeo.SitemapSettings.Exclude) return null;

        var externalUrl = GetExternalUrl(page);
        if (string.IsNullOrEmpty(externalUrl)) return null;

        // Add page URL
        var sitemapElement = new XElement(
            SitemapXmlNamespace + "url",
            new XElement(SitemapXmlNamespace + "loc", externalUrl));

        // add language links
        foreach (var lang in pageBaseSeo.ExistingLanguages)
        {
            var linkNode = new XElement(XhtmlNamespace + "link",
                new XAttribute("rel", "alternate"),
                new XAttribute("hreflang", lang.TwoLetterISOLanguageName),
                new XAttribute("href", GetExternalUrl(page, lang.TwoLetterISOLanguageName)));
            sitemapElement.Add(linkNode);
        }

        // add lastmod
        if (pageBaseSeo.StartPublish != null)
        {
            var sitemapLastmodElement = new XElement(SitemapXmlNamespace + "lastmod",
                pageBaseSeo.StartPublish.Value.ToString("yyyy-MM-dd"));
            sitemapElement.Add(sitemapLastmodElement);
        }

        if (!string.IsNullOrEmpty(pageBaseSeo.SitemapSettings.Priority))
        {
            // try to get number
            decimal.TryParse(pageBaseSeo.SitemapSettings.Priority, out var priority);

            if (priority > 0)
            {
                // add priority
                var sitemapPriorityElement =
                    new XElement(new XElement(SitemapXmlNamespace + "priority",
                        priority.ToString(CultureInfo.GetCultureInfo("en"))));
                sitemapElement.Add(sitemapPriorityElement);
            }
        }

        return sitemapElement;
    }

    private string GetExternalUrl(ContentReference page, string language = null)
    {
        if (ContentReference.IsNullOrEmpty(page)) return string.Empty;

        return _urlResolver.GetUrl(page, language, new UrlResolverArguments { ContextMode = EPiServer.Web.ContextMode.Default, ForceAbsolute = true });
    }
}
