using Org.BouncyCastle.Utilities.IO.Pem;

public class MailSettings
{
    public static string MailOptionsKey = "MailSettings";
    public string Host { get; set; } = string.Empty;

    public int Port { get; set; }

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}