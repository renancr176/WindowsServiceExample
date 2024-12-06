using InnokuMailSender.Interfaces.Services;
using InnokuMailSender.Models.SmtpService;
using InnokuMailSender.Options;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace InnokuMailSender.Services;

public class SmtpService : ISmtpService, IDisposable
{
    private readonly IOptions<SmtpOptions> _options;
    private readonly ILogService _logService;

    public SmtpService(
        IOptions<SmtpOptions> options,
        ILogService logService)
    {
        _options = options;
        _logService = logService;
    }

    private SmtpOptions SmtpOptions => _options.Value;

    private SmtpClient _smtpClient;
    private SmtpClient SmtpClient { 
        get {
            if (_smtpClient != null)
                return _smtpClient;

            _smtpClient = new SmtpClient(SmtpOptions.Host, SmtpOptions.Port);
            _smtpClient.Credentials = new NetworkCredential(SmtpOptions.User, SmtpOptions.Password);
            _smtpClient.EnableSsl = SmtpOptions.EnableSsl;

            return _smtpClient;
        } 
    }

    private List<string> GeneratedFiles = new List<string>();

    public async Task<bool> Send(SmtpDataModel model)
    {
        SmtpClient smtpClient = null;

        try
        {
            if (model.SmtpDataConnection != null)
            {
                smtpClient = new SmtpClient(model.SmtpDataConnection.Host, model.SmtpDataConnection.Port);
                smtpClient.Credentials = new NetworkCredential(model.SmtpDataConnection.User, model.SmtpDataConnection.Password);
                smtpClient.EnableSsl = model.SmtpDataConnection.EnableSsl;
            }

            var from = new MailAddress(model.From.Email, model.From.Name, System.Text.Encoding.UTF8);
            var to = new MailAddress(model.To.Email, model.To.Name, System.Text.Encoding.UTF8);

            var message = new MailMessage(from, to);
            message.Subject = model.Subject;
            message.Body = model.Body;
            message.BodyEncoding = message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = model.IsHtml;

            foreach (var cc in (model.Cc ?? new List<SmtpDataMailAddressModel>()).Where(x => !string.IsNullOrEmpty(x.Email?.Trim())))
            {
                message.CC.Add(new MailAddress(cc.Email, cc.Name, System.Text.Encoding.UTF8));
            }

            foreach (var bcc in (model.Bcc ?? new List<SmtpDataMailAddressModel>()).Where(x => !string.IsNullOrEmpty(x.Email?.Trim())))
            {
                message.Bcc.Add(new MailAddress(bcc.Email, bcc.Name, System.Text.Encoding.UTF8));
            }

            foreach (var attachment in model.Attachments ?? new List<SmtpDataAttachment>())
            {
                var filePath = $"{Path.GetTempPath()}{Path.DirectorySeparatorChar}{Guid.NewGuid()}{Path.GetExtension(attachment.FileName)}";
                await File.WriteAllBytesAsync(filePath, attachment.File);
                GeneratedFiles.Add(filePath);
                new FileExtensionContentTypeProvider().TryGetContentType(filePath, out var mediaType);
                var contentType = new System.Net.Mime.ContentType();
                contentType.MediaType = mediaType;
                contentType.Name = attachment.FileName;
                message.Attachments.Add(new Attachment(filePath, contentType));
            }

            if (smtpClient == null)
            {
                SmtpClient.Send(message);
            } 
            else
            {
                smtpClient.Send(message);
            }

            return true;
        }
        catch (Exception ex)
        {
            await _logService.LogAsync(ex);
        }
        finally
        {
            if (smtpClient != null)
            {
                try { smtpClient.Dispose(); } catch { }
            }
        }

        return false;
    }

    public void Dispose()
    {
        if (_smtpClient != null)
        {
            _smtpClient.Dispose();
            _smtpClient = null;
        }

        foreach (var file in GeneratedFiles)
        {
            if (File.Exists(file))
            {
                try { File.Delete(file); } catch { }
            }
        }
    }
}
