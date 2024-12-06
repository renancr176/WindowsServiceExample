namespace InnokuMailSender.Interfaces.Services;

public interface IFtpService
{
    Task<byte[]?> DownloadAsync(string filePath);
}
