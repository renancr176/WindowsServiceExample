using System.ComponentModel.DataAnnotations;

namespace InnokuMailSender.Models.SmtpService;

public class SmtpDataMailAddressModel
{
    [EmailAddress]
    public string Email { get; set; }
    public string? Name { get; set; }

    public SmtpDataMailAddressModel(string email, string? name = null)
    {
        Email = email;
        Name = name;
    }
}
