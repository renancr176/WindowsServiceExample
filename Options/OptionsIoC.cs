using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InnokuMailSender.Options;

public static class OptionsIoC
{
    public static void AddOptionsIoC(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<SmtpOptions>()
            .BindConfiguration(SmtpOptions.sectionKey)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<LogServiceOptions>()
            .BindConfiguration(LogServiceOptions.sectionKey)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<FtpOptions>()
            .BindConfiguration(FtpOptions.sectionKey)
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}
