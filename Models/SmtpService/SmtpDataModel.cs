namespace InnokuMailSender.Models.SmtpService;

public class SmtpDataModel
{
    public SmtpDataConnectionModel SmtpDataConnection { get; set; }
    public SmtpDataMailAddressModel From { get; set; }
    public SmtpDataMailAddressModel To { get; set; }
    public IEnumerable<SmtpDataMailAddressModel>? Cc { get; set; } = new List<SmtpDataMailAddressModel>();
    public IEnumerable<SmtpDataMailAddressModel>? Bcc { get; set; } = new List<SmtpDataMailAddressModel>();
    public string Subject { get; set; } 
    public string Body { get; set; }
    public bool IsHtml { get; set; } = true;
    public IEnumerable<SmtpDataAttachment> Attachments { get; set; } = new List<SmtpDataAttachment>();

    public SmtpDataModel(
        SmtpDataMailAddressModel from, 
        SmtpDataMailAddressModel to, 
        string subject, 
        string body,
        bool isHtml,
        SmtpDataConnectionModel smtpDataConnection = null)
    {
        From = from;
        To = to;
        Subject = subject;
        Body = body;
        IsHtml = isHtml;
        SmtpDataConnection = smtpDataConnection;
    }

    public SmtpDataModel(
        SmtpDataMailAddressModel from, 
        SmtpDataMailAddressModel to, 
        string subject, 
        string body,
        bool isHtml,
        SmtpDataConnectionModel smtpDataConnection = null,
        IEnumerable<SmtpDataMailAddressModel>? cc = null, 
        IEnumerable<SmtpDataMailAddressModel>? bcc = null)
        : this (from, to, subject, body, isHtml, smtpDataConnection)
    {
        Cc = cc ?? new List<SmtpDataMailAddressModel>();
        Bcc = bcc ?? new List<SmtpDataMailAddressModel>();
    }

    public SmtpDataModel(
        SmtpDataMailAddressModel from, 
        SmtpDataMailAddressModel to, 
        string subject, 
        string body,
        bool isHtml,
        IEnumerable<SmtpDataAttachment> attachments,
        SmtpDataConnectionModel smtpDataConnection = null)
        : this(from, to, subject, body, isHtml, smtpDataConnection)
    {
        Attachments = attachments ?? new List<SmtpDataAttachment>();
    }

    public SmtpDataModel(SmtpDataMailAddressModel from, 
        SmtpDataMailAddressModel to, 
        string subject, 
        string body,
        bool isHtml,
        IEnumerable<SmtpDataAttachment> attachments,
        SmtpDataConnectionModel smtpDataConnection = null,
        IEnumerable<SmtpDataMailAddressModel>? cc = null, 
        IEnumerable<SmtpDataMailAddressModel>? bcc = null)
        : this(from, to, subject, body, isHtml, smtpDataConnection, cc, bcc)
    {
        Attachments = attachments ?? new List<SmtpDataAttachment>();
    }
}
