using EPiServer;
using EPiServer.Core;

namespace Optimizely.Demo.Core.Models.ViewModels;

public record ContentRenderingErrorModel
{
    public ContentRenderingErrorModel(IContentData contentData, Exception exception)
    {
        if (contentData is IContent content)
        {
            ContentName = content.Name;
        }
        else
        {
            ContentName = string.Empty;
        }

        ContentTypeName = contentData.GetOriginalType().Name;

        Exception = exception;
    }

    public string ContentName { get; set; }

    public string ContentTypeName { get; set; }

    public Exception Exception { get; set; }
}
