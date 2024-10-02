namespace Optimizely.Demo.Integrations.Services.RetryService;

internal interface IRetryService
{
    void Do(Action action);
    T Do<T>(Func<T> action) where T : class;
    Task DoAsync(Func<Task> action);
    Task<T> DoAsync<T>(Func<Task<T>> action) where T : class;
}
