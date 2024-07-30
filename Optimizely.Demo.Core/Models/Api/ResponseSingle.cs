namespace Optimizely.Demo.Core.Models.Api;

public record ResponseSingle<T> : ResponseBase where T : class
{
    public T? Result { get; init; }
}
