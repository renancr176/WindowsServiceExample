namespace InnokuMailSender.Models.LogService;

public class LogMessage
{
    public string Message { get; set; }
    public LogLevelEnum? LogLevel { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;

    public LogMessage(string message, LogLevelEnum? logLevel = null)
    {
        Message = message;
        LogLevel = logLevel;
    }
}
