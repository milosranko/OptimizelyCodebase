using EPiServer.Core;
using EPiServer.Framework.Localization;
using System.ComponentModel.DataAnnotations;

namespace Optimizely.Demo.ContentTypes.Attributes.Validations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public sealed class ContentAreaMaxItemCount : ValidationAttribute
{
    private readonly int _limit;
    public int Limit => _limit;

    public ContentAreaMaxItemCount(int limit)
    {
        _limit = limit;
    }

    public override bool IsValid(object value)
    {
        return ValidateContentArea(value as ContentArea);
    }

    public override string FormatErrorMessage(string name)
    {
        return LocalizationService.Current.GetString("/validations/contentareamaxitems");
    }

    private bool ValidateContentArea(ContentArea contentArea)
    {
        if (contentArea == null || contentArea.Items == null || !contentArea.Items.Any())
            return true;

        return contentArea.Items.Count <= Limit;
    }
}
