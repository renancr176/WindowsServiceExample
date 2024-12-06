using System.ComponentModel.DataAnnotations;

namespace InnokuMailSender.Options;

public class SmtpOptions
{
    public static string sectionKey = "SMTP";
    [Required]
    [MinLength(1)]
    public string Host { get; set; }
    [Required]
    [Range(80, 9999)]
    public int Port { get; set; }
    public bool EnableSsl { get; set; }
    [Required]
    [MinLength(1)]
    public string User { get; set; }
    [Required]
    [MinLength(1)]
    public string Password { get; set; }
    public Uri Uri => new Uri($"{Host}:{Port}");
}

