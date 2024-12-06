namespace InnokuMailSender.Models.SmtpService;

public class SmtpDataConnectionModel
{
    public string Host { get; set; }
    public int Port { get; set; }
    public bool EnableSsl { get; set; } = true;
    public string User { get; set; }
    public string Password { get; set; }

    public SmtpDataConnectionModel(string host, int port, string user, string password, bool enableSsl = true)
    {
        Host = host;
        Port = port;
        EnableSsl = enableSsl;
        User = user;
        Password = password;
    }
}
