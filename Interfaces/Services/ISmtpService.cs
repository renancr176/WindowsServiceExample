using InnokuMailSender.Models.SmtpService;

namespace InnokuMailSender.Interfaces.Services;

public interface ISmtpService
{
    Task<bool> Send(SmtpDataModel model);
}
