namespace Optimizely.Demo.Core.Models.Api;

public abstract record ResponseBase
{
	public IEnumerable<string>? Errors { get; set; }
}
