using EPiServer.Core;
using System.Text.RegularExpressions;

namespace Optimizely.Demo.ContentTypes.Extensions;

public static class XhtmlStringExtensions
{
    /// <summary>
    /// Strip xhtml strings of all html elements
    /// </summary>
    /// <param name="xhtml"></param>
    /// <returns>A stripped string</returns>
    public static string StripXhtml(this XhtmlString xhtml)
    {
        if (xhtml == null) return string.Empty;

        return Regex.Replace(xhtml.ToString(), @"<(.|\n)*?>", string.Empty);
    }

    /// <summary>
    /// Strip xhtml of all html elements and return selected lengt of string
    /// </summary>
    /// <returns>A stripped and cropped string</returns>
    public static string StripXhtml(this XhtmlString xhtml, int length)
    {
        return xhtml.StripXhtml(length, false);
    }

    /// <summary>
    /// Strip xhtml of all html elements and return selected lengt of string
    /// </summary>
    /// <param name="xhtml"></param>
    /// <param name="length"></param>
    /// <param name="smartcrop">Crop string at blank space</param>
    /// <returns>A stripped and cropped string</returns>
    public static string StripXhtml(this XhtmlString xhtml, int length, bool smartcrop)
    {
        var plaintext = xhtml.StripXhtml();

        // Crop and return string            
        return plaintext.CropString(length, smartcrop);
    }
}
