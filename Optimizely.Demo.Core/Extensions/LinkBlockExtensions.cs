using Optimizely.Demo.Core.Models.Blocks.Local;

namespace Optimizely.Demo.ContentTypes.Extensions;

public static class LinkBlockExtensions
{
    public static bool IsNullOrEmpty(this LinkBlock block)
    {
        return (block == null) || (block != null && (string.IsNullOrEmpty(block.LinkText) || block.LinkUrl == null));
    }
}
