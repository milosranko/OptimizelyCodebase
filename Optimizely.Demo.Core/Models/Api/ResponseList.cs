namespace Optimizely.Demo.Core.Models.Api;

public record ResponseList<T> : ResponseBase where T : class
{
	public IEnumerable<T>? Results { get; init; }
	public int Page { get; set; }
	public int PageSize { get; set; }
	public int TotalNumberOfItems { get; set; }
}
