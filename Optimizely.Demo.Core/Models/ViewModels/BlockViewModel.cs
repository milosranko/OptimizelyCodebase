using Optimizely.Demo.ContentTypes.Helpers;
using Optimizely.Demo.ContentTypes.Models.Blocks.Base;
using System.Text.Json.Serialization;

namespace Optimizely.Demo.Core.Models.ViewModels;

public record BlockViewModel<TBlock> where TBlock : BlockBase
{
    [JsonIgnore]
    public TBlock? CurrentBlock { get; }
    public bool IsEditing => PageHelpers.IsInEditMode();
}
