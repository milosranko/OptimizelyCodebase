using System.Collections.ObjectModel;
using System.Net;

namespace Optimizely.Demo.Integrations.Services.RetryService;

internal class RetryService
{
    private static RetryPolicyConfig RetryPolicy { get; set; }

    public static void CreateInstance(RetryPolicyConfig retryPolicy)
    {
        RetryPolicy = retryPolicy;
    }

    public void Do(Action action)
    {
        if (!RetryPolicy.Enabled) action();

        var exceptions = new Collection<Exception>();

        for (var attempted = 0; attempted < RetryPolicy.MaxAttempts; attempted++)
        {
            try
            {
                if (attempted > 0)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(RetryPolicy.RetryIntervalInMilliseconds));
                }

                action();
            }
            catch (Exception e)
            {
                exceptions.Add(e);

                if (!IsWorthRetry(e))
                    break;
            }
        }

        throw new AggregateException(exceptions);
    }

    public async Task DoAsync(Func<Task> action)
    {
        if (!RetryPolicy.Enabled) await action();

        var exceptions = new Collection<Exception>();

        for (var attempted = 0; attempted < RetryPolicy.MaxAttempts; attempted++)
        {
            try
            {
                if (attempted > 0)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(RetryPolicy.RetryIntervalInMilliseconds));
                }

                await action();
            }
            catch (Exception e)
            {
                exceptions.Add(e);

                if (!IsWorthRetry(e))
                    break;
            }
        }

        throw new AggregateException(exceptions);
    }

    public T Do<T>(Func<T> action) where T : class
    {
        if (!RetryPolicy.Enabled) return action();

        var exceptions = new Collection<Exception>();

        for (var attempted = 0; attempted < RetryPolicy.MaxAttempts; attempted++)
        {
            try
            {
                if (attempted > 0)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(RetryPolicy.RetryIntervalInMilliseconds));
                }

                return action();
            }
            catch (Exception e)
            {
                exceptions.Add(e);

                if (!IsWorthRetry(e))
                    break;
            }
        }

        throw new AggregateException(exceptions);
    }

    public async Task<T> DoAsync<T>(Func<Task<T>> action) where T : class
    {
        if (!RetryPolicy.Enabled) return await action();

        var exceptions = new Collection<Exception>();

        for (var attempted = 0; attempted < RetryPolicy.MaxAttempts; attempted++)
        {
            try
            {
                if (attempted > 0)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(RetryPolicy.RetryIntervalInMilliseconds));
                }

                return await action();
            }
            catch (Exception e)
            {
                exceptions.Add(e);

                if (!IsWorthRetry(e))
                    break;
            }
        }

        throw new AggregateException(exceptions);
    }

    private static bool IsWorthRetry(Exception e)
    {
        return e is TimeoutException || e is HttpRequestException || e is WebException;
    }
}
