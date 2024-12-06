using InnokuMailSender.Interfaces.Services;
using InnokuMailSender.Models.LogService;
using InnokuMailSender.Options;
using Microsoft.Extensions.Options;
using System.Net;

namespace InnokuMailSender.Services;

public class FtpService : IFtpService
{
    private readonly IOptions<FtpOptions> _options;
    private readonly ILogService _logService;

    public FtpService(
        IOptions<FtpOptions> options,
        ILogService logService)
    {
        _options = options;
        _logService = logService;

        if (_options.Value.Uri.Scheme != Uri.UriSchemeFtp)
        {
            var message = $"The informed {nameof(FtpOptions.Host)} and/or {FtpOptions.Port} is not a valid FTP.";
            _logService.LogAsync(new LogMessage(message, LogLevelEnum.ERROR));
            throw new ArgumentException(nameof(FtpOptions.Uri), message);
        }
    }

    private FtpOptions FtpOptions => _options.Value;
    private WebClient WebClient => new WebClient()
    {
        BaseAddress = FtpOptions.Uri.ToString(),
        Credentials = new NetworkCredential(FtpOptions.User, FtpOptions.Password)
    };

    public async Task<byte[]?> DownloadAsync(string filePath)
    {
        try
        {
            return WebClient.DownloadData(filePath);
        }
        catch (Exception ex)
        {
            await _logService.LogAsync(ex);
        }
        return default;
    }
}
