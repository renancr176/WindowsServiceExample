using InnokuMailSender.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InnokuMailSender.Services;

public static class ServicesIoC
{
    public static void AddServicesIoC(this IServiceCollection services)
    {
        services.AddScoped<IFtpService, FtpService>();
        services.AddScoped<ILogService, LogService>();
        services.AddScoped<ISmtpService, SmtpService>();
    }
}
