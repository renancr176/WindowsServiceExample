using InnokuMailSender.Interfaces.Services;
using InnokuMailSender.Models.LogService;
using InnokuMailSender.Options;
using Microsoft.Extensions.Options;
using Serilog;
using System.Text.Json;

namespace InnokuMailSender.Services;

public class LogService : ILogService, IDisposable
{
    private readonly IOptions<LogServiceOptions> _options;

    public LogService(IOptions<LogServiceOptions> options)
    {
        _options = options;
    }

    private LogServiceOptions LogServiceOptions => _options.Value;

    private Serilog.Core.Logger _serilogSinksFile;
    private Serilog.Core.Logger SerilogSinksFile
    {
        get
        {
            if (_serilogSinksFile != null)
                return _serilogSinksFile;

            _serilogSinksFile = new LoggerConfiguration()
            .WriteTo.File(LogServiceOptions.FullFilePath, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
            .CreateLogger();

            return _serilogSinksFile;
        }
    }

    public async Task LogAsync(LogMessage log)
    {
        var formatedMessage = $"{log.Date.ToString("G")} ==> {log.LogLevel}: {log.Message}";
        SerilogSinksFile.Write(
            log.LogLevel.HasValue && log.LogLevel == LogLevelEnum.ERROR
                ? Serilog.Events.LogEventLevel.Error
                : log.LogLevel.HasValue && log.LogLevel == LogLevelEnum.WARN
                    ? Serilog.Events.LogEventLevel.Warning
                    : Serilog.Events.LogEventLevel.Information,
            log.Message);

        Console.WriteLine(formatedMessage);
    }

    public async Task LogAsync(Exception ex)
    {
        var formatedMessage = $"{DateTime.Now.ToString("G")} ==> ERROR: {JsonSerializer.Serialize(new { ex.Message, ex.StackTrace })}";
        SerilogSinksFile.Write(
            Serilog.Events.LogEventLevel.Error,
            JsonSerializer.Serialize(new { ex.Message, ex.StackTrace }));

        Console.WriteLine(formatedMessage);
    }

    public void Dispose()
    {
        if (_serilogSinksFile != null)
        {
            _serilogSinksFile.Dispose();
            _serilogSinksFile = null;
        }
    }
}
