using EPiServer.Core;
using EPiServer.Core.Html.StringParsing;
using EPiServer.Framework.Localization;
using System.ComponentModel.DataAnnotations;

namespace Optimizely.Demo.ContentTypes.Attributes.Validations;

[AttributeUsage(AttributeTargets.Property)]
public class AllowedTypesForXhtml : ValidationAttribute
{
    private readonly Type[] _allowedTypes;
    private string _typeName = string.Empty;

    public AllowedTypesForXhtml(params Type[] allowedTypes)
    {
        var typeArray = allowedTypes ?? new[]
        {
                typeof(IContentData)
            };

        _allowedTypes = typeArray;
    }

    public override bool IsValid(object value)
    {
        if (value == null || !(value is XhtmlString xHtml)) return true;
        if (!xHtml.Fragments.OfType<ContentFragment>().Any()) return true;

        var contentItems = xHtml.Fragments.OfType<ContentFragment>().Select(x => x.GetContent());
        var isAllowed = false;

        foreach (var item in contentItems)
        {
            isAllowed = false;

            foreach (var type in AllowedTypes)
            {
                var memberInfo = item.GetType().BaseType;
                isAllowed = memberInfo != null && (memberInfo.IsAssignableFrom(type) || memberInfo.IsSubclassOf(type));

                if (isAllowed)
                    break;
            }

            if (isAllowed) continue;
            _typeName = item.Name;
            return false;
        }

        return isAllowed;
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{LocalizationService.Current.GetString("/validations/allowedtypesforxhtmlerror")}{_typeName}";
    }

    public Type[] AllowedTypes => (Type[])_allowedTypes.Clone();
}
