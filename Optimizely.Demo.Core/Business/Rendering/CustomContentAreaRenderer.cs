using EPiServer.Core;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using EPiServer.Web.Mvc.Html;
using EPiServer.Web.Templating;
using Microsoft.AspNetCore.Mvc.Rendering;
using Optimizely.Demo.ContentTypes.Models.Blocks.Interfaces;

namespace Optimizely.Demo.Core.Business.Rendering;

public class CustomContentAreaRenderer : ContentAreaRenderer
{
    private readonly IContentAreaLoader _contentAreaLoader;
    private readonly IContentRenderer _contentRenderer;
    private readonly IContentAreaItemAttributeAssembler _attributeAssembler;
    private readonly ContentAreaRenderingOptions _contentAreaRenderingOptions;

    public CustomContentAreaRenderer(
        IContentAreaLoader contentAreaLoader,
        IContentRenderer contentRenderer,
        IContentAreaItemAttributeAssembler attributeAssembler,
        ContentAreaRenderingOptions contentAreaRenderingOptions)
    {
        _contentAreaLoader = contentAreaLoader;
        _contentRenderer = contentRenderer;
        _attributeAssembler = attributeAssembler;
        _contentAreaRenderingOptions = contentAreaRenderingOptions;
    }

    protected override void RenderContentAreaItem(
        IHtmlHelper htmlHelper,
        ContentAreaItem contentAreaItem,
        string templateTag,
        string htmlTag,
        string cssClass)
    {
        var renderSettings = new Dictionary<string, object>
        {
            ["childrencustomtagname"] = htmlTag,
            ["childrencssclass"] = cssClass,
            ["tag"] = templateTag
        };

        renderSettings = contentAreaItem.RenderSettings.Concat(
            from r in renderSettings
            where !contentAreaItem.RenderSettings.ContainsKey(r.Key)
            select r).ToDictionary(r => r.Key, r => r.Value);

        htmlHelper.ViewBag.RenderSettings = renderSettings;

        if (_contentAreaLoader.LoadContent(contentAreaItem) is not IContent content)
        {
            return;
        }

        var templateTags = Enumerable.Empty<string>();
        var contentAreaTemplateTag = GetContentAreaTemplateTag(htmlHelper);
        templateTags = ((!string.IsNullOrWhiteSpace(templateTag)) ? ((_contentAreaRenderingOptions.TemplateTagSelectionStrategy != 0 && !string.IsNullOrEmpty(contentAreaTemplateTag)) ? new string[2] { templateTag, contentAreaTemplateTag } : new string[1] { templateTag }) : new string[1] { contentAreaTemplateTag });

        var templateModel = ResolveContentTemplate(htmlHelper, content, templateTags);

        if (templateModel != null || IsInEditMode())
        {
            var renderWrappingElement = ShouldRenderWrappingElementForContentAreaItem(htmlHelper);
            using (new ContentRenderingScope(htmlHelper.ViewContext.HttpContext, content))
            {
                // The code for this method has been c/p from EPiServer.Web.Mvc.Html.ContentAreaRenderer.
                // The only difference is this if/else block.
                if (IsInEditMode() || renderWrappingElement)
                {
                    var tagBuilder = new TagBuilder(htmlTag);

                    AddNonEmptyCssClass(tagBuilder, cssClass);
                    tagBuilder.MergeAttributes(_attributeAssembler.GetAttributes(
                        contentAreaItem,
                        IsInEditMode(),
                        templateModel != null));
                    BeforeRenderContentAreaItemStartTag(tagBuilder, contentAreaItem);
                    htmlHelper.ViewContext.Writer.Write(tagBuilder.RenderStartTag());
                    htmlHelper.RenderContentData(content, true, templateModel, _contentRenderer);
                    htmlHelper.ViewContext.Writer.Write(tagBuilder.RenderEndTag());
                }
                else
                {
                    // This is the extra code: don't render wrapping elements in view mode
                    htmlHelper.RenderContentData(content, true, templateModel, _contentRenderer);
                }
            }
        }
    }

    protected override bool ShouldRenderWrappingElement(IHtmlHelper htmlHelper)
    {
        // set 'hascontainer' to false by default
        var item = (bool?)htmlHelper.ViewContext.ViewData["hascontainer"];
        return item.HasValue && item.Value;
    }

    protected override string GetContentAreaItemHtmlTag(IHtmlHelper htmlHelper, ContentAreaItem contentAreaItem)
    {
        if (contentAreaItem.LoadContent() is IContentAreaCustomHtml content)
        {
            return string.IsNullOrEmpty(content?.ChildrenCustomTagName)
                ? base.GetContentAreaItemHtmlTag(htmlHelper, contentAreaItem)
                : content.ChildrenCustomTagName;
        }

        return base.GetContentAreaItemHtmlTag(htmlHelper, contentAreaItem);
    }

    protected override string GetContentAreaItemCssClass(IHtmlHelper htmlHelper, ContentAreaItem contentAreaItem)
    {
        var cssClass = base.GetContentAreaItemCssClass(htmlHelper, contentAreaItem);
        return GetTypeSpecificCssClasses(contentAreaItem, cssClass);
    }

    private static string GetTypeSpecificCssClasses(ContentAreaItem contentAreaItem, string cssClass)
    {
        if (string.IsNullOrWhiteSpace(cssClass) && contentAreaItem.LoadContent() is IContentAreaCustomHtml content && !string.IsNullOrWhiteSpace(content.ChildrenCssClass))
        {
            cssClass += $"{(string.IsNullOrEmpty(cssClass) ? "" : " ")}{content.ChildrenCssClass}";
        }

        return cssClass;
    }

    private bool ShouldRenderWrappingElementForContentAreaItem(IHtmlHelper htmlHelper)
    {
        // set 'haschildcontainers' to false by default
        var item = (bool?)htmlHelper.ViewContext.ViewData["haschildcontainers"];
        return item.HasValue && item.Value;
    }
}
