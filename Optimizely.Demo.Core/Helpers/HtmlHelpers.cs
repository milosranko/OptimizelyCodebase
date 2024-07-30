using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.Web;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Mvc.Html;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Optimizely.Demo.ContentTypes.Extensions;
using Optimizely.Demo.ContentTypes.Models.Blocks.Base;
using Optimizely.Demo.ContentTypes.Models.Media.Base;
using System.Linq.Expressions;
using System.Text;
using System.Text.Encodings.Web;
using System.Web;

namespace Optimizely.Demo.ContentTypes.Helpers;

public static class HtmlHelpers
{
    #region Meta helper

    public static void MetaTag(this IHtmlHelper helper, string name, string value)
    {
        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
        {
            helper.ViewContext.Writer.WriteLine("<meta name=\"{0}\" content=\"{1}\" />", name, value);
        }
    }

    public static void OpenGraphTag(this IHtmlHelper helper, string name, string value)
    {
        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
        {
            helper.ViewContext.Writer.WriteLine("<meta property=\"{0}\" content=\"{1}\" />", name, value);
        }
    }

    #endregion

    #region Menu helper

    /// <summary>
    /// Returns an element for each child page of the rootLink using the itemTemplate.
    /// </summary>
    /// <param name="helper">The html helper in whose context the list should be created</param>
    /// <param name="rootLink">A reference to the root whose children should be listed</param>
    /// <param name="itemTemplate">A template for each page which will be used to produce the return value. Can be either a delegate or a Razor helper.</param>
    /// <param name="includeRoot">Wether an element for the root page should be returned</param>
    /// <param name="requireVisibleInMenu">Wether pages that do not have the "Display in navigation" checkbox checked should be excluded</param>
    /// <param name="requirePageTemplate">Wether page that do not have a template (i.e. container pages) should be excluded</param>
    /// <remarks>
    /// Filter by access rights and publication status.
    /// </remarks>
    public static IHtmlContent MenuList(
        this IHtmlHelper helper,
        ContentReference rootLink,
        Func<MenuItem, HelperResult> itemTemplate = null,
        bool includeRoot = false,
        bool requireVisibleInMenu = true,
        bool requirePageTemplate = true)
    {
        itemTemplate = itemTemplate ?? GetDefaultItemTemplate(helper);
        var currentContentLink = helper.ViewContext.HttpContext.GetContentLink();
        var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();

        Func<IEnumerable<PageData>, IEnumerable<PageData>> filter =
            pages => pages.FilterForDisplay(requirePageTemplate, requireVisibleInMenu);

        var pagePath = contentLoader.GetAncestors(currentContentLink)
            .Reverse()
            .Select(x => x.ContentLink)
            .SkipWhile(x => !x.CompareToIgnoreWorkID(rootLink))
            .ToList();

        var menuItems = contentLoader.GetChildren<PageData>(rootLink)
            .FilterForDisplay(requirePageTemplate, requireVisibleInMenu)
            .Select(x => CreateMenuItem(x, currentContentLink, pagePath, contentLoader, filter))
            .ToList();

        if (includeRoot)
        {
            menuItems.Insert(0, CreateMenuItem(contentLoader.Get<PageData>(rootLink), currentContentLink, pagePath, contentLoader, filter));
        }

        var buffer = new StringBuilder();
        var writer = new StringWriter(buffer);
        foreach (var menuItem in menuItems)
        {
            itemTemplate(menuItem).WriteTo(writer, HtmlEncoder.Default);
        }

        return new HtmlString(buffer.ToString());
    }

    private static MenuItem CreateMenuItem(
        PageData page,
        ContentReference currentContentLink,
        List<ContentReference> pagePath,
        IContentLoader contentLoader,
        Func<IEnumerable<PageData>,
            IEnumerable<PageData>> filter)
    {
        var menuItem = new MenuItem(page)
        {
            Selected = page.ContentLink.CompareToIgnoreWorkID(currentContentLink) ||
                       pagePath.Contains(page.ContentLink),
            HasChildren = new Lazy<bool>(() => filter(contentLoader.GetChildren<PageData>(page.ContentLink)).Any())
        };
        return menuItem;
    }

    private static Func<MenuItem, HelperResult> GetDefaultItemTemplate(IHtmlHelper helper)
    {
        return x => new HelperResult(writer =>
        {
            helper.PageLink(x.Page).WriteTo(writer, HtmlEncoder.Default);
            return Task.CompletedTask;
        });
    }

    public class MenuItem
    {
        public MenuItem(PageData page)
        {
            Page = page;
        }
        public PageData Page { get; set; }
        public bool Selected { get; set; }
        public Lazy<bool> HasChildren { get; set; }
    }

    #endregion

    #region EditableField

    public static IHtmlContent EditableField<TModel, TResult>(
        this IHtmlHelper<TModel> model,
        Expression<Func<TModel, TResult>> expression,
        string tagName,
        object? htmlAttributes = null,
        bool addPlaceholder = false,
        string? tags = null)
    {
        var expressionProvider = new ModelExpressionProvider(model.MetadataProvider);
        var metadata = expressionProvider.CreateModelExpression(model.ViewData, expression);
        var hasValue = metadata.Model != null;

        // edit mode (with or without value)
        if (PageHelpers.IsInEditMode())
        {
            return RenderPropertyForEditMode<TModel, TResult>(tagName, htmlAttributes, metadata, model, addPlaceholder, tags);
        }
        // view mode with value
        if (hasValue)
        {
            return RenderPropertyForViewMode<TModel, TResult>(tagName, htmlAttributes, metadata, model, tags);
        }
        // view mode without value
        return HtmlString.Empty;
    }

    private static IHtmlContent RenderPropertyForEditMode<TModel, TResult>(
        string tagName,
        object htmlAttributes,
        ModelExpression metadata,
        IHtmlHelper htmlHelper,
        bool addPlaceholder,
        string tags)
    {
        var imageInlineEditTag = new TagBuilder("div");
        var tagBuilder = new TagBuilder(tagName);
        var cssClass = string.Empty;
        RouteValueDictionary? attributes = null;

        if (htmlAttributes != null)
        {
            attributes = new RouteValueDictionary(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        if (!addPlaceholder)
        {
            tagBuilder.MergeAttribute("data-epi-property-name", metadata.Metadata.PropertyName);
            tagBuilder.MergeAttribute("data-epi-use-mvc", "True");
        }
        else
        {
            imageInlineEditTag.MergeAttribute("class", $"epi-editContainer editContainerImage {cssClass}");
            imageInlineEditTag.MergeAttribute("data-epi-property-name", metadata.Metadata.PropertyName);
            imageInlineEditTag.MergeAttribute("data-epi-overlay-z-index", "-1");
            imageInlineEditTag.MergeAttribute("data-epi-use-mvc", "True");
        }

        if (metadata.Model != null && !tagName.Equals("img"))
        {
            if (metadata.ModelExplorer.ModelType == typeof(string))
            {
                var text = metadata.Model.ToString();

                if (metadata.Metadata.TemplateHint == UIHint.Textarea)
                {
                    tagBuilder.InnerHtml.SetHtmlContent(text.ConvertNewLineToBR());
                }
                else
                {
                    tagBuilder.InnerHtml.SetContent(text);
                }
            }
            else if (metadata.ModelExplorer.ModelType == typeof(ContentReference) && metadata.Metadata.TemplateHint == UIHint.Block)
            {
                tagBuilder.InnerHtml.SetHtmlContent(RenderBlockByContentReference(htmlHelper, metadata.Model as ContentReference, tags));
            }
            else
            {
                tagBuilder.InnerHtml.SetContent(metadata.Model.ToString());
            }
        }
        else if (tagName.Equals("img"))
        {
            tagBuilder.TagRenderMode = TagRenderMode.SelfClosing;

            if (metadata.Model != null)
            {
                SetImageAttributes((ContentReference)metadata.Model, ref attributes);
            }
        }

        tagBuilder.MergeAttributes(attributes);

        if (tagName.Equals("img"))
        {
            if (addPlaceholder && metadata.Model != null)
            {
                imageInlineEditTag.InnerHtml.SetHtmlContent(tagBuilder);
                return imageInlineEditTag;
            }
        }

        return tagBuilder.ToHtmlString();
    }

    private static IHtmlContent RenderPropertyForViewMode<TModel, TResult>(
        string tagName,
        object htmlAttributes,
        ModelExpression metadata,
        IHtmlHelper htmlHelper,
        string tags)
    {
        RouteValueDictionary? attributes = null;
        var tagBuilder = new TagBuilder(tagName);

        if (htmlAttributes != null)
        {
            attributes = new RouteValueDictionary(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        if (!tagName.Equals("img"))
        {
            if (metadata.ModelExplorer.ModelType == typeof(string) && metadata.Metadata.TemplateHint == UIHint.Textarea)
            {
                tagBuilder.InnerHtml.SetHtmlContent(metadata.Model.ToString().ConvertNewLineToBR());
            }
            else if (metadata.ModelExplorer.ModelType == typeof(ContentReference) && metadata.Metadata.TemplateHint == UIHint.Block)
            {
                tagBuilder.InnerHtml.SetHtmlContent(RenderBlockByContentReference(htmlHelper, metadata.Model as ContentReference, tags));
            }
            else
            {
                tagBuilder.InnerHtml.SetContent(metadata.Model.ToString());
            }
        }
        else if (metadata.ModelExplorer.ModelType == typeof(ContentReference) && !ContentReference.IsNullOrEmpty((ContentReference)metadata.Model))
        {
            tagBuilder.TagRenderMode = TagRenderMode.SelfClosing;
            SetImageAttributes((ContentReference)metadata.Model, ref attributes);
        }

        tagBuilder.MergeAttributes(attributes);

        return tagBuilder.ToHtmlString();
    }

    private static HtmlString ToHtmlString(this TagBuilder tagBuilder)
    {
        using (var writer = new StringWriter())
        {
            tagBuilder.WriteTo(writer, HtmlEncoder.Default);
            return new HtmlString(HttpUtility.HtmlDecode(writer.ToString()));
        }
    }

    private static void SetImageAttributes(
        ContentReference imageRef,
        ref RouteValueDictionary attributes)
    {
        var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
        var urlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();
        var image = contentLoader.Get<ImageFileBase>(imageRef);
        var imageAlt = image.AlternateText ?? string.Empty;
        var imageUrl = urlResolver.GetUrl(imageRef);
        var src = imageUrl;

        if (attributes["srcset"] != null)
        {
            var srcSet = (string)attributes["srcset"];
            src = imageUrl.GetSrc(srcSet.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).First(), (string)attributes["mode"]);

            if (attributes["sizes"] != null)
            {
                attributes["srcset"] = imageUrl.GetSrcSet(srcSet, (string)attributes["mode"]);
            }
            else
            {
                attributes.Remove("srcset");
            }
        }

        attributes.Add("src", src);
        attributes.Add("alt", imageAlt);

        if (attributes["mode"] != null)
        {
            attributes.Remove("mode");
        }
    }

    private static IHtmlContent? RenderBlockByContentReference(
        this IHtmlHelper htmlHelper,
        ContentReference blockContentReference,
        string tags)
    {
        if (blockContentReference == ContentReference.EmptyReference)
        {
            return null;
        }

        var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
        var block = contentRepository.Get<BlockBase>(blockContentReference);

        if (block == null)
        {
            return null;
        }

        var templateResolver = ServiceLocator.Current.GetInstance<TemplateResolver>();
        var httpContext = ServiceLocator.Current.GetInstance<IHttpContextAccessor>().HttpContext;
        var template = templateResolver.Resolve(httpContext, block, TemplateTypeCategories.Request, tags);

        return HtmlHelperPartialExtensions.Partial(htmlHelper, template.Name, block);
    }

    #endregion

    #region Colored heading

    public static IHtmlContent ApplyStyle(this string text, string tagName = "span", string className = "")
    {
        var parsed = string.Empty;

        if (!string.IsNullOrEmpty(text))
        {
            if (text.Contains("*-") && text.Contains("-*"))
            {
                var cssClass = !string.IsNullOrEmpty(className) ? $" class=\"{className}\"" : "";
                parsed = text.Replace("*-", $"<{tagName}{cssClass}>").Replace("-*", $"</{tagName}>");
            }
            else
            {
                parsed = text;
            }
        }

        return ApplyNewLine(parsed);
    }

    public static IHtmlContent ApplyNewLine(this string text)
    {
        return text.ConvertNewLineToBR();
    }

    #endregion
}
