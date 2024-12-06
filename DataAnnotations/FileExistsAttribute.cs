using System.ComponentModel.DataAnnotations;

namespace InnokuMailSender.DataAnnotations;

public class FileExistsAttribute : LocalPathAttribute
{
    public override bool IsValid(object? value)
    {
        if (base.IsValid(value))
        {
            var strValue = value?.ToString()?.Trim();
            var fileExtension = Path.GetExtension(strValue);
            return !string.IsNullOrEmpty(fileExtension) && File.Exists(strValue);
        }
        return false;
    }
}
