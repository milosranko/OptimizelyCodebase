using Serilog;

namespace Optimizely.Demo.PublicWeb;

public class Program
{
    public static IConfiguration Configuration { get; } =
            new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appSettings.json", false, true)
            .AddJsonFile($"appSettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true)
            .AddEnvironmentVariables()
            .Build();

    public static void Main(string[] args)
    {
        Console.WriteLine("Starting up...");
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureCmsDefaults()
            .UseSerilog((ctx, provider, lc) => lc.ReadFrom.Configuration(ctx.Configuration))
            .ConfigureAppConfiguration((ctx, builder) =>
            {
                builder.AddConfiguration(Configuration);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>().UseUrls("https://localhost:5000", "https://localhost:5001");
                webBuilder.UseStaticWebAssets();
            });
}