using Microsoft.Extensions.Hosting;

namespace InnokuMailSender;

public class WindowsBackgroundService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
    }
}
