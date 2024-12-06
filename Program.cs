using InnokuMailSender.Options;
using InnokuMailSender.Scheduler;
using InnokuMailSender.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace InnokuMailSender;

internal class Program
{
    static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .UseWindowsService(options =>
        {
            options.ServiceName = "Innoku - InnokuMailSender";
        })
        .ConfigureAppConfiguration((hostContext, x) =>
        {
            x.SetBasePath(hostContext.HostingEnvironment.ContentRootPath);
            x.AddJsonFile("appsettings.json", false);
            x.AddJsonFile("appsettings.Development.json", true);
        })
        .ConfigureLogging(loggingConfiguration => loggingConfiguration.ClearProviders())
        .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration))
        .ConfigureServices(static (hostContext, services) =>
        {
            services.AddHostedService<WindowsBackgroundService>();

            services.AddSingleton<IConfiguration>(hostContext.Configuration);
            //services.AddDb(hostContext.Configuration);
            services.AddOptionsIoC(hostContext.Configuration);
            services.AddServicesIoC();
            services.AddScheduler(hostContext.Configuration);
        });
}