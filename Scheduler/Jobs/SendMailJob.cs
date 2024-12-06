using InnokuMailSender.Interfaces.Services;
using InnokuMailSender.Models.LogService;
using InnokuMailSender.Options;
using Microsoft.Extensions.Options;
using Quartz;
using System.Diagnostics;

namespace InnokuMailSender.Scheduler.Jobs;

[DisallowConcurrentExecution]
public class SendMailJob : IJob
{
    private readonly ILogService _logService;

    public SendMailJob(
        ILogService logService)
    {
        _logService = logService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            await _logService.LogAsync(new LogMessage($"{nameof(SendMailJob)} started.", LogLevelEnum.INFO));
            //TODO: Run some service.
            await _logService.LogAsync(new LogMessage($"{nameof(SendMailJob)} ended.", LogLevelEnum.INFO));
        } 
        catch (Exception ex)
        {
            await _logService.LogAsync(ex);
        }
    }
}
