using System.ComponentModel.DataAnnotations;

namespace InnokuMailSender.Options;

public class FtpOptions
{
    public static string sectionKey = "FTP";
    [Required]
    [MinLength(1)]
    [Url]
    public string Host { get; set; }
    [Required]
    [Range(1000, 9999)]
    public int Port { get; set; }
    public bool UseSll { get; set; }
    [Required]
    [MinLength(1)]
    public string User { get; set; }
    [Required]
    [MinLength(1)]
    public string Password { get; set; }
    public Uri Uri => new Uri($"{Host}:{Port}");
}
