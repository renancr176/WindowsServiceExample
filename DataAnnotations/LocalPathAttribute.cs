using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace InnokuMailSender.DataAnnotations;

public class LocalPathAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        var strValue = value?.ToString()?.Trim();
        if (!string.IsNullOrEmpty(strValue))
        {
            strValue = Regex.Replace(strValue, @"/\\+/", @"\");
            try
            {
                var uri = new Uri(strValue);
                return uri.IsFile;
            }
            catch { }
        }

        return false;
    }
}
