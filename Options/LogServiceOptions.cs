using InnokuMailSender.DataAnnotations;
using System.Text.RegularExpressions;

namespace InnokuMailSender.Options;

public class LogServiceOptions
{
    public static string sectionKey = "LogService";
    public bool LogToFile { get; set; }
    public string? LogFileName { get; set; }
    public string? LogFilePath { get; set; }
    public string FullFilePath => $"{(!string.IsNullOrEmpty(LogFilePath) ? LogFilePath : Directory.GetCurrentDirectory())}{Path.DirectorySeparatorChar}{LogFileName ?? "LOD_MAIL_SENDER.txt"}";
}
