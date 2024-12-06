using InnokuMailSender.Models.LogService;

namespace InnokuMailSender.Interfaces.Services;

public interface ILogService
{
    Task LogAsync(LogMessage message);
    Task LogAsync(Exception ex);
}
