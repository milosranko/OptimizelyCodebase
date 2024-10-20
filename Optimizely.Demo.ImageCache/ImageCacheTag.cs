﻿using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Optimizely.Demo.ImageCache;

[HtmlTargetElement("image-cache")]
public class ImageCacheTag : TagHelper
{
    [HtmlAttributeName("css")]
    public string? Css { get; set; }

    [HtmlAttributeName("style")]
    public string? Style { get; set; }

    [HtmlAttributeName("srcset")]
    public string? SrcSet { get; set; }

    [HtmlAttributeName("sizes")]
    public string? Sizes { get; set; }

    [HtmlAttributeName("model")]
    public ModelExpression Model { get; set; }

    [HtmlAttributeName("altfallback")]
    public string? AltFallback { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (Model.Metadata == null)
        {
            output.Content.Clear();
            return;
        }

        if (Model.Metadata.ModelType == typeof(ContentReference) && !ContentReference.IsNullOrEmpty((ContentReference)Model.Model))
        {
            output.TagName = "img";
            output.TagMode = TagMode.SelfClosing;

            SetImageAttributes((ContentReference)Model.Model, output.Attributes);
        }
    }

    private void SetImageAttributes(ContentReference imageRef, TagHelperAttributeList attributes)
    {
        var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
        //var urlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();
        var image = contentLoader.Get<ImageData>(imageRef);
        var media = image.;
        var imageAlt = media.AltText ?? string.Empty;
        var imageUrl = media.PublicUrl.Remove(0, 1);
        var src = imageUrl;

        if (!string.IsNullOrEmpty(SrcSet))
        {
            src = imageUrl.GetSrc(SrcSet.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).First());

            if (!string.IsNullOrEmpty(Sizes))
            {
                attributes.Add("srcset", imageUrl.GetSrcSet(SrcSet));
            }
        }

        attributes.Add("src", src);

        if (!string.IsNullOrEmpty(imageAlt))
        {
            attributes.Add("alt", imageAlt);
        }
        else if (!string.IsNullOrEmpty(AltFallback))
        {
            attributes.Add("alt", AltFallback);
        }

        if (!string.IsNullOrEmpty(Css))
        {
            attributes.Add("class", Css);
        }

        if (!string.IsNullOrEmpty(Style))
        {
            attributes.Add("style", Style);
        }

        if (!string.IsNullOrEmpty(Sizes))
        {
            attributes.Add("sizes", Sizes);
        }
    }
}
