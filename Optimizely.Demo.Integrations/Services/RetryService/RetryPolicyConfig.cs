namespace Optimizely.Demo.Integrations.Services.RetryService;

internal record RetryPolicyConfig
{
    public bool Enabled { get; set; } = true;
    public int RetryIntervalInMilliseconds { get; set; } = 500;
    public int MaxAttempts { get; set; } = 3;
}
