using Microsoft.AspNetCore.Html;
using Optimizely.Demo.ContentTypes.Helpers;
using System.Globalization;
using System.Text;

namespace Optimizely.Demo.ContentTypes.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Truncates string at whole word to specific length
    /// </summary>
    /// <param name="input"></param>
    /// <param name="length"></param>
    /// <returns>Truncated string</returns>
    public static string TruncateAtWord(this string input, int length)
    {
        if (input == null || input.Length < length)
            return input;

        var iNextSpace = input.LastIndexOf(" ", length);

        return $"{input.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim()}...";
    }

    public static string GetMonthName(this string monthNumber)
    {
        return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(monthNumber));
    }

    public static string CropString(this string text, int length, bool smartcrop)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;

        // If string is shorter than desired length, return entire string
        if (length >= text.Length)
            return text;

        // If string should use smart crop, set length to index of closest blank space
        if (smartcrop)
            length = text.LastIndexOf(" ", length, length);

        // Crop and return string            
        return text.Substring(0, length) + "...";
    }

    public static string Replace(this string originalString, string oldValue, string newValue, StringComparison comparisonType)
    {
        int startIndex = 0;
        while (true)
        {
            startIndex = originalString.IndexOf(oldValue, startIndex, comparisonType);
            if (startIndex == -1)
                break;

            originalString = originalString.Substring(0, startIndex) + newValue + originalString.Substring(startIndex + oldValue.Length);

            startIndex += newValue.Length;
        }

        return originalString;
    }

    public static string RemoveAccents(this string input)
    {
        if (!string.IsNullOrEmpty(input))
        {
            return new string(
                input
                .Normalize(NormalizationForm.FormD)
                .ToCharArray()
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray());
        }
        // the normalization to FormD splits accented letters in accents+letters
        // the rest removes those accents (and other non-spacing characters)

        return string.Empty;
    }

    public static string GetSrcSet(this string imageUrl, string srcSet = null, string mode = null)
    {
        if (srcSet == null)
            return imageUrl;

        var breakingPoints = srcSet.Split('|');
        var result = new StringBuilder();

        foreach (var point in breakingPoints)
        {
            var dim = point.Split('x');
            var width = dim[0];
            var height = string.Empty;

            if (dim.Length == 2)
            {
                height = "&h=" + dim[1];
            }

            result
                .AppendFormat($"{imageUrl}{(PageHelpers.IsInEditMode() ? "&" : "?")}w={width}{height}{(mode != null ? "&mode=" + mode : string.Empty)} {width}w, ");
        }

        return result.ToString().TrimEnd(' ', ',');
    }

    public static string GetSrc(this string imageUrl, string srcSet = null, string mode = null)
    {
        var result = new StringBuilder();

        if (srcSet == null)
            return imageUrl;

        var dim = srcSet.Split('x');
        var width = dim[0];
        var height = string.Empty;

        if (dim.Length == 2)
        {
            height = "&h=" + dim[1];
        }

        return result
            .AppendFormat($"{imageUrl}{(PageHelpers.IsInEditMode() ? "&" : "?")}w={width}{height}{(mode != null ? "&mode=" + mode : string.Empty)}")
            .ToString();
    }

    public static IHtmlContent ConvertNewLineToBR(this string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            return new HtmlString(text.Replace("\n", "<br />"));
        }

        return HtmlString.Empty;
    }

    public static string ToUpperFirstLetter(this string text)
    {
        return string.IsNullOrEmpty(text) ? string.Empty : text.Remove(0, 1).Insert(0, char.ToUpperInvariant(text[0]).ToString());
    }
}
