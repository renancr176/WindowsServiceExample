namespace InnokuMailSender.Models.SmtpService;

public class SmtpDataAttachment
{
    public string FileName { get; set; }
    public byte[] File { get; set; }

    public SmtpDataAttachment(string fileName, byte[] file)
    {
        FileName = fileName;
        File = file;
    }
}
